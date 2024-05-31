using System;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using BepInEx;
using BepInEx.Logging;
using Aki.Common.Http;
using Comfort.Common;
using VisualQuestTree.UI;
using VisualQuestTree.Patches;
using EFT.UI;

namespace VisualQuestTree
{
    public class Quest {
        public string id;
        public string name;
        public List<string> parents;
        public List<string> children;
        public int LevelRequirement;
        public string traderId;
        public (string, int) repRequirements = (null,0);
        public bool isTreeRoot = false;
        public int status;

        public Quest(string i, string n, List<string> p, List<string> c, int l, string t, (string,int) rep, int s){
            id = i;
            name = n;
            parents = p;
            children = c;
            LevelRequirement = l;
            traderId = t;
            repRequirements = rep;
            status = s;
        }

        public void setRoot(bool b) {
            isTreeRoot = b;
        }
    }
    public class Trader {
        public string id;
        public string name;
        public List<Quest> quests;

        public Trader(string n, string i){
            name = n;
            id = i;
            quests = new List<Quest>{};
        }
    }

    [BepInPlugin("com.dewdongers.visualquesttree", "Visual Quest Tree", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public static ManualLogSource Log => Instance.Logger;
        public QuestTreeScreen QuestsScreen;
        public List<Trader> Traders;
        public List<Quest> MasterQuestList;

        void Awake()
        {
            Instance = this;
            Log.LogInfo($"Visual Quest Tree loaded.");

            Instance.LoadQuests();

            new CommonUIAwakePatch().Enable();
            new QuestScreenShowPatch().Enable();
            new QuestScreenClosePatch().Enable();
        }    

        public void LoadQuests()
        {
            Log.LogInfo($"Loading quests");
            JArray questData = JArray.Parse(RequestHandler.GetJson("/VisualQuestTreeRoutes/quests"));
            JArray traderData = JArray.Parse(RequestHandler.GetJson("/VisualQuestTreeRoutes/traders"));

            Traders = new List<Trader>();
            MasterQuestList = new List<Quest>();

            foreach (JObject item in traderData)
            {
                Trader trader = new Trader(item.GetValue("name").ToString(), item.GetValue("id").ToString());
                Log.LogInfo("Trader: " + item.GetValue("name").ToString());
                Traders.Add(trader);
            }

            Log.LogInfo("Initialize Quests");
            foreach (JObject item in questData)
            {
                // Initialize quest values. More can be added at a future date.
                string traderId = (string)item["traderId"];
                string questID = (string)item["_id"];
                string questName = (string)item["QuestName"];
                List<string> parents = new List<string>();
                List<string> children = new List<string>();
                (string, int) repReq = (null,0);
                int questStatus = 0;
                int questLevelReq = 1;

                var conditioncount = (JArray)item["conditions"]["AvailableForStart"];
                for( int i = 0; i < conditioncount.Count; i++) {
                    // Iterate over all starting conditions, sort into Level Requirement and Quest Requirement.
                    string startingconditions = (string)item["conditions"]["AvailableForStart"][i]["conditionType"];
                    if(startingconditions.ToString() == "Level")
                    {
                        questLevelReq = (int)item["conditions"]["AvailableForStart"][i]["value"];
                    }
                    else if (startingconditions.ToString() == "Quest")
                    {
                        string target = (string)item["conditions"]["AvailableForStart"][i]["target"];
                        parents.Add(target);
                    }
                    else if (startingconditions.ToString() == "TraderLoyalty" || startingconditions.ToString() == "TraderStanding")
                    {
                        string traderRepTarget = (string)item["conditions"]["AvailableForStart"][i]["target"];
                        int traderRepValue = (int)item["conditions"]["AvailableForStart"][i]["value"];
                        repReq = (traderRepTarget,traderRepValue);
                    }
                    else
                    {
                        Log.LogError("Unhandled Starting Condition: " + questName);
                    }
                }
                
                var tempQuest = new Quest(questID,questName,parents,children,questLevelReq,traderId,repReq,questStatus);
                MasterQuestList.Add(tempQuest);
            }
            
            Log.LogInfo("Populate Quest Children");
            foreach (Quest q in MasterQuestList) {
                foreach (string p in q.parents) {
                    var questParentMatch = MasterQuestList.FirstOrDefault(x => x.id == p);
                    if(questParentMatch != null) 
                    {
                        questParentMatch.children.Add(q.id);
                    } else {
                        Log.LogError($"Quest Child cannot be assigned to Quest Parent: " + q.name + " Cannot assign to parent - " + p);
                    }
                }
            }

            Log.LogInfo("Assign Quests to Traders");
            foreach (Quest q in MasterQuestList) {
                string questTraderId = q.traderId;
                var tradermatch = Traders.FirstOrDefault(x => x.id == questTraderId);
                if(tradermatch != null)
                {
                    tradermatch.quests.Add(q);
                }
                else
                {
                    Log.LogError($"Quest cannot be assigned to trader: " + q.name);
                }
            }

            GetQuestStatus();

            Log.LogInfo($"\n\n\n Trader Debugging");
            foreach(Trader t in Traders) {
                Log.LogInfo($""+t.name+": ");
                foreach(Quest q in t.quests){
                    Log.LogInfo("\t" + q.name + " - LVL " + q.LevelRequirement);
                    Log.LogInfo("\t\t" + "Parents");
                    foreach(string p in q.parents){
                        Log.LogInfo("\t\t\t" + LookUpQuestNameByID(p));
                    } 
                    Log.LogInfo("\t\t" + "Children");
                    foreach(string c in q.children) {
                        Log.LogInfo("\t\t\t" + LookUpQuestNameByID(c));
                    }
                }
            }
        }


        public string LookUpQuestNameByID(string id) 
        {
            try {
                return MasterQuestList.FirstOrDefault(x => x.id == id).name;
            } catch (Exception e) {
                return "LOOKUPFAILED - " + id;
            }
        }

        public void GetQuestStatus() 
        {
            Log.LogInfo("Get Quests Status");
            JArray questStatusData = JArray.Parse(RequestHandler.GetJson("/VisualQuestTreeRoutes/questStatus"));
            foreach(JObject item in questStatusData) 
            {
                string id = (string)item["id"];
                int status = (int)item["status"];
                Log.LogInfo("ID: " + id + " " + status);
                MasterQuestList.FirstOrDefault(x => x.id == id).status = status;
                
            }
        }

        internal void TryAttachToTasksScreen(TasksScreen tasksScreen) 
        {
            Log.LogInfo($"TryAttach");
            if (QuestsScreen != null)
            {
                return;
            }

            Log.LogInfo("Trying to attach to TasksScreen");

            // attach to common UI first to call awake and set things up, then attach to sleeping tasks screen
            QuestsScreen = QuestTreeScreen.Create(Singleton<CommonUI>.Instance.gameObject);
            QuestsScreen.transform.SetParent(tasksScreen.transform);
        }
    }
}