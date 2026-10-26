# 诸葛瞻·移动版

> 此版本与 `classic:zhuge-zhan` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，蜀、男、3体力；完整引用既有经典技能，不复制定义、状态或执行器。

来源：[移动官网341](https://www.sanguosha.cn/pc/hero-detail-341.html)、[移动人物38211](https://wiki.biligame.com/msgs/index.php?title=诸葛瞻&oldid=38211)、[罪论35531](https://wiki.biligame.com/msgs/index.php?title=罪论&oldid=35531)、[父荫38999](https://wiki.biligame.com/msgs/index.php?title=父荫&oldid=38999)。普通身份与国战、界版及其他版本分别核查。

## 诸葛瞻：首次目标账本与罪论原实体私看已有完整路径

官网341与当前移动正文一致：父荫只对每实际回合首次被杀/决斗指定生效，比较双方当前手牌，自己不多于使用者时该牌对自己无效。罪论结束时私看顶3，按造成伤害、未弃牌、手牌全场最少三项决定获得数，并排序剩余；零项时与另一角色各失1。

现有 `classic:fuyin` 用原生 `cardUseTargetsFinalized` 与实际指定目标，不依赖一个“条件成功才消耗”的触发次数。`GameEngine.QuotaTopPrograms.cs.RegisterFirstTurnTargetActions` 在查是否启用技能及比较手牌前，就以真实ActionId记录首次遇到；随后 `IsFirstTurnTarget` 精确匹配原牌的父帧和目标。首次手牌条件不成立、暂时被压制或失源后重新获得，均不会把第二张误当首次。手牌比较和无效化发生在该原生牌上，额度按TurnNumber，不被额外Play重置。

罪论的同文件 `ComputeTurnCriterionQuota` 读取真实正伤害、`GetProgramDiscardSource` 的逻辑弃牌来源和存活角色手牌最小值。交牌/用牌不被冒充弃牌；`GameEngine.ProgramDiscardOrigins.cs` 只认可真实弃置理由和其Processing完成链。私看用实际顶牌实体，选择取得后等待移动、回到原 `QuotaTop` 帧排序；零配额的双方失HP有原帧签发与子返回协议，未新造平行状态。

最小落地仍是纯Content引用。已有 `FengLinZhugeZhanChecks.PrivateQuotaAndTopReplay` 覆盖真实交牌不算弃牌、私密顶3、冻结获得数、剩余顶序和重放，可复用作批次回归；不应为移动版再写同一套定义快照。


纯内容引用由共享加载、注册和机制检查覆盖，不新增重复人物夹具。

经典立绘来自[官网45800.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/45800.jpg)，本地 `official-mobile-zhuge-zhan.jpg`，图鉴分组 `myth-thunder`。
