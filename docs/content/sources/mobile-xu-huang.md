# 徐晃·移动版

> 此版本与 `classic:xu-huang` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，魏、男、4体力、神话再临林包。完整引用已有 `classic:duanliang`，只增加人物内容，不复制技能定义或运行时。

来源：[移动官网42](https://www.sanguosha.cn/pc/hero-detail-42.html)、[移动BWIKI人物34819](https://wiki.biligame.com/msgs/index.php?title=徐晃&oldid=34819)、[断粮20620](https://wiki.biligame.com/msgs/index.php?title=断粮/徐晃&oldid=20620)。本次通过MediaWiki revisions API重读当前原始修订：人物2025-12-05、技能2023-01-04，仍分别为34819/20620；普通栏为林包、160将魂合成或招募获取。当前技能页同时列出国战及历史版本，本文只采用“经典”身份规则。

普通断粮将一张黑色非锦囊牌当兵粮寸断使用，并允许对距离2的其他角色使用兵粮。官网“黑色基本牌或黑色装备牌”与普通技能页“黑色非锦囊牌”在当前牌类别中一致。没有出牌阶段限一次、每花色限次、造成伤害前提、截辎，亦没有国战的无距离及距离大于2后本回合禁用。

现有 `_shared/Skills/conversion-cutover.rules.json` 的 `classic:duanliang` 使用一张HE牌，类别Basic/Equipment、花色Spade/Club，原生转换为SupplyShortage，仅供实际Play使用；`cardUseDistanceLimit +1` 将原生兵粮的距离1扩至2。没有按界徐晃本回合造成伤害情况或目标手牌数另行判定。

转换继续走既有原生CardUse：原装备或手牌作为真实物理材料支付，支付后目标及距离仍按共享资格校验，实体进入原生延时锦囊位置，原生无懈、判定、typed子返回及cold协议保持。没有复制或伪造一张兵粮，也没有将已接受的装备费用重新视作手牌成本。本次完全复用已有能力，沿共享加载/转换/延时锦囊检查，不新增重复人物夹具；构建与批次验证由主代理统一执行。

普通经典图：[4200.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4200.jpg)，官网42第二张；首张44300为界版。已与移动Wiki“徐晃-经典形象.jpg”视觉核对一致。`portraitKey` 为 `mobile-xu-huang`，图鉴分组 `myth-forest`，人物及将池序号86699。
