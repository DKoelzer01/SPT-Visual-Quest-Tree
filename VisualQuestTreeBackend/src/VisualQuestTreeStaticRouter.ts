import { DependencyContainer } from "tsyringe";
import type { IPreAkiLoadMod } from "@spt-aki/models/external/IPreAkiLoadMod";
import type { ILogger } from "@spt-aki/models/spt/utils/ILogger";
import type {DynamicRouterModService} from "@spt-aki/services/mod/dynamicRouter/DynamicRouterModService";
import type {StaticRouterModService} from "@spt-aki/services/mod/staticRouter/StaticRouterModService";
import { DatabaseServer } from "@spt-aki/servers/DatabaseServer";
import { ProfileHelper } from "@spt-aki/helpers/ProfileHelper";
//import { QuestConditionHelper } from "@spt-aki/helpers/QuestConditionHelper";
import { QuestHelper } from "@spt-aki/helpers/QuestHelper";
import { IQuest } from "@spt-aki/models/eft/common/tables/IQuest"
import { IPmcData } from "@spt-aki/models/eft/common/IPmcData"
import { IQuestConfig } from "@spt-aki/models/spt/config/IQuestConfig";
import { ConfigTypes } from "@spt-aki/models/enums/ConfigTypes";
import { ConfigServer } from "@spt-aki/servers/ConfigServer";
import { Traders } from "@spt-aki/models/enums/Traders";
import { TraderHelper } from "@spt-aki/helpers/TraderHelper";
import { FenceService } from "@spt-aki/services/FenceService";
import { ITemplateItem } from "@spt-aki/models/eft/common/tables/ITemplateItem";
import { ILocaleBase } from "@spt-aki/models/spt/server/ILocaleBase";

class Trader {
    protected name = "";
    protected id = ""

    constructor(name,id){
        this.name = name;
        this.id = id;
    }
}

class Mod implements IPreAkiLoadMod
{
    protected questConfig: IQuestConfig;
	
    public preAkiLoad(container: DependencyContainer): void {
        const logger = container.resolve<ILogger>("WinstonLogger");
        const dynamicRouterModService = container.resolve<DynamicRouterModService>("DynamicRouterModService");
        const staticRouterModService = container.resolve<StaticRouterModService>("StaticRouterModService");
        const profileHelper = container.resolve<ProfileHelper>("ProfileHelper");
        const questHelper = container.resolve<QuestHelper>("QuestHelper");
		const configServer = container.resolve<ConfigServer>("ConfigServer");
        this.questConfig = configServer.getConfig(ConfigTypes.QUEST);
        //const questConditionHelper = container.resolve<QuestConditionHelper>("QuestConditionHelper");
        const traderHelper = container.resolve<TraderHelper>("TraderHelper");
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
        const fenceService = container.resolve<FenceService>("FenceService");

        // Hook up a new static route
        staticRouterModService.registerStaticRouter(
            "VisualQuestTreeRoutes",
            [
                {
                    url: "/VisualQuestTreeRoutes/quests",
                    action: (url, info, sessionID, output) => 
                    {
                        logger.info("Visual Quest Tree quest data request");
						const quests: IQuest[] = [];
						const allQuests = questHelper.getQuestsFromDb();

						const profile: IPmcData = profileHelper.getPmcProfile(sessionID);
						
						if(profile && profile.Quests)
						{
							for (const quest of allQuests)
							{
								// Skip if not a quest we can have
								if (profile.Info && this.questIsForOtherSide(profile.Info.Side, quest._id))
								{
									continue;
								}
								
								const questStatus = questHelper.getQuestStatus(profile, quest._id);

								/*
								Locked = 0,
								AvailableForStart = 1,
								Started = 2,
								AvailableForFinish = 3,
								Success = 4,
								Fail = 5,
								FailRestartable = 6,
								MarkedAsFailed = 7,
								Expired = 8,
								AvailableAfter = 9
								
								if (questStatus >= 3 && questStatus <= 8)
								{
									continue;
								}
								*/

                                if (questHelper.getQuestFromDb(quest._id,profile).name.contains("event")) {
                                    continue;
                                }

								quests.push(quest);
							}
							logger.info("Visual Quest Tree Got quests");
						}
						else
						{
							logger.info("Unable to fetch quests for Visual Quest Tree");
						}
						
						return JSON.stringify(quests);
                    }
                },
                {
                    url: "/VisualQuestTreeRoutes/traders",
                    action: (url, info, sessionID, output) => 
                    {
                        //Vanilla Traders
                        const Prapor = new Trader("Prapor","54cb50c76803fa8b248b4571");
                        const Therapist = new Trader("Therapist","54cb57776803fa99248b456e");
                        const Fence = new Trader("Fence","579dc571d53a0658a154fbec");
                        const Skier = new Trader("Skier","58330581ace78e27b8b10cee");
                        const Peacekeeper = new Trader("Peacekeeper","5935c25fb3acc3127c3d8cd9");
                        const Mechanic = new Trader("Mechanic","5a7c2eca46aef81a7ca2145d");
                        const Ragman = new Trader("Ragman","5ac3b934156ae10c4430e83c");
                        const Jaeger = new Trader("Jaeger","5c0647fdd443bc2504c2d371");
                        const Lightkeeper = new Trader("Lightkeeper","638f541a29ffd1183d187f57");

                        const BTR = new Trader("BTR","656f0f98d80a697f855d34b1");
                        const Caretaker = new Trader("Caretaker","638f541a29ffd1183d187f57");
                        
                        //Modded Traders
                        const Artem = new Trader("Artem","ArtemTrader");
                        const Scorpion = new Trader("Scorpion","Scorpion");
                        const Lotus = new Trader("Lotus","lotus");

                        const traders: Trader[] = [Prapor,Therapist,Fence,Skier,Peacekeeper,Mechanic,Ragman,Jaeger,Lightkeeper,BTR,Caretaker,Artem,Scorpion,Lotus]; //Add additional traders here.
                        return JSON.stringify(traders);
                    }
                },
            ],
            "custom-static-VisualQuestTreeRoutes"
        );
        
    }
	
    protected questIsForOtherSide(playerSide: string, questId: string): boolean
    {
        const isUsec = playerSide.toLowerCase() === "usec";
        if (isUsec && this.questConfig.bearOnlyQuests.includes(questId))
        {
            // player is usec and quest is bear only, skip
            return true;
        }

        if (!isUsec && this.questConfig.usecOnlyQuests.includes(questId))
        {
            // player is bear and quest is usec only, skip
            return true;
        }

        return false;
    }
}
module.exports = {mod: new Mod()}