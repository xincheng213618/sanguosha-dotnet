# 姜维·移动版

> 此版本与 `classic:jiang-wei` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，蜀、男、4体力；完整引用既有经典技能，不复制定义、状态或执行器。

来源：[移动官网52](https://www.sanguosha.cn/pc/hero-detail-52.html)、[移动人物35428](https://wiki.biligame.com/msgs/index.php?title=姜维&oldid=35428)、[挑衅40313](https://wiki.biligame.com/msgs/index.php?title=挑衅/姜维&oldid=40313)、[志继38617](https://wiki.biligame.com/msgs/index.php?title=志继/姜维&oldid=38617)、[观星40227](https://wiki.biligame.com/msgs/index.php?title=观星/姜维&oldid=40227)。普通身份与国战、界版及其他版本分别核查。

## 姜维：真实每出牌阶段一次，原生杀与觉醒均可复用

当前普通挑衅专页于2026-08-28更新：每个出牌阶段一次，目标必须是攻击范围内包含自己的其他角色；目标对自己使用杀或让自己弃置其一张牌。它没有界OL版本“杀必须造成伤害、失败后本阶段限两次”的机制，也不是界移动版不受目标攻击范围限制的挑衅。

`jiang-wei/classic/Skills/classic-jiang-wei.rules.json` 的 `classic:tiaoxin` 精确声明 `usesPerTurn:null / usesPerPhase:1` 和 `otherLivingWhoseAttackRangeIncludesOwner`。`GameEngine.EnterPlayPhase` 在真实新出牌阶段入口清除该座位 `_programPhaseUses`，所以额外Play重新可用，不是用技能文案伪装turn限次。

`requestSlashByTarget` 在 `GameEngine.ProgramSlashRequest.cs` 冻结原请求帧与真实出杀者、验证所选手牌实体，先记录 `used-slash`，再以原卡执行原生杀并返回原程序；因此这张杀被闪避仍算已使用，只有拒绝/没有合法杀时才走弃牌。后续取得目标HE实体经 `selectAndMoveOwnedCard` 原生弃置，`skipIfNoCards` 不造空弃牌。真实请求响应、杀的父帧和已付费恢复沿现共享协议，不需新人物pending。

普通志继在准备阶段无手牌时强制觉醒且每局一次，先选择回复1或摸2，再减1上限、获得观星。现 `classic:zhiji` 的游戏次数、窗口和节点顺序相同；使用共用技能授予原语。移动[观星40227](https://wiki.biligame.com/msgs/index.php?title=观星/姜维&oldid=40227) 2026-08-27正文为存活人数、最多5张、自由顶底，与 `_shared/Skills/judgment-cutover.rules.json` 的 `classic:guanxing` 一致。

最小落地仅新增移动Content与永久来源文档，`skills:[]`，不复制挑衅、志继或观星定义，不新建重复觉醒/响应矩阵。若单独做人物夹具，须同时登记授予依赖的观星来源；生产全内容注册已有该ID。


纯内容引用由共享加载、注册和机制检查覆盖，不新增重复人物夹具。

经典立绘来自[官网5200.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/5200.jpg)，本地 `official-mobile-jiang-wei.jpg`，图鉴分组 `myth-mountain`。
