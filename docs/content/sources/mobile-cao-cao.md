# 曹操·移动版

> 此版本与 `classic:cao-cao` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。当前普通移动身份，魏、男、4体力，标准包；完整引用 `classic:jianxiong` 和 `classic:hujia`，没有复制技能定义或运行时。

来源：[移动官网15](https://www.sanguosha.cn/pc/hero-detail-15.html)、[移动BWIKI人物34823](https://wiki.biligame.com/msgs/index.php?title=曹操&oldid=34823)（2025-12-05）、[普通奸雄30691](https://wiki.biligame.com/msgs/index.php?title=奸雄/曹操&oldid=30691)（2025-06-01）、[普通护驾26603](https://wiki.biligame.com/msgs/index.php?title=护驾/曹操&oldid=26603)（2024-03-28）。本次直接读取以上当前MediaWiki修订；人物经典栏给出9级等级奖励，属于已上线普通标准身份。

普通奸雄在受到一次伤害后可获得造成该伤害的牌，没有额外摸牌，也没有按伤害点重复获得的收益。既有 `_shared/Skills/damage-skills.rules.json` 使用 `afterDamageApplied/subject:owner/perDamage` 与 `claimDamageCards`；`GetClaimableProgramDamageCards` 沿原伤害帧取得实际物理材料，仅取仍在Processing的原实体，并由原生移动放入本人Hand。群体锦囊的已被取得材料也有原群体帧记录，不能按牌名重新生成或从别处取回。

普通护驾完整引用 `_shared/Skills/passive-response-rules.rules.json` 的 `factionResponseRequest`：主公需要使用或打出闪时，请其他魏势力角色依次决定是否响应。原策略保留主公资格，按当前有效势力选择其他提供者，不把本人当提供者；提供者实际打出的闪、转换来源与原始受益者分属现有原生响应凭据，不把提供者误当被攻击目标。该版本没有界护驾附加收益或额外费用。

纯人物配置由现有加载、注册和共享机制检查覆盖，不新增重复夹具。既有 `ShanheStartingResourceChecks.ClaimedResourcesAndLegacyRecipeRetainTheirPhysicalCards` 直接检查经典奸雄取得原伤害实体；`CardResponseCompletionCostChecks.TypedHealthKindMatchesActualResponseCostRecovery` 含同一魏势力主公请求策略的真实闪付款、体力子窗与原响应返回。这里沿用已有覆盖，本次未执行构建或测试。

普通经典立绘：[官网1500.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1500.jpg)，在官网15列表的第二张；首张31100为界曹操图，不能代替普通身份。`portraitKey` 为 `mobile-cao-cao`，本地目标名 `official-mobile-cao-cao.jpg`，图鉴分组 `standard`，人物注册及将池序号84199。没有新增操作码。
