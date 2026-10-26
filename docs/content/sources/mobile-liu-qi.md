# 移动版刘琦

> 此版本与 `ol:liu-qi` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。正式身份 `mobile:liu-qi`，群、男性、基础三体力，人物 `character:liu-qi`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-liu-qi`，注册与当前将池顺序81299。移动Wiki归属SP武将组5，图库使用 `sp`。

[移动官网256](https://www.sanguosha.cn/pc/hero-detail-256.html)与[移动BWIKI当前刘琦](https://wiki.biligame.com/msgs/刘琦)（oldid37575，2026-03-18更新）普通经典身份正文一致。问计在自己的出牌阶段开始时，可令一名其他角色自主交一张牌给刘琦；刘琦在这个实际回合内使用与所交牌同名的牌时，其他角色不能响应。屯江在自己的结束阶段检查本回合没有跳过出牌阶段，且自己的出牌阶段内没有使用牌指定其他角色为目标，然后摸当前全场势力种类数的牌。人物当前经典栏有合成与招募获取方式，非测试或爆料身份；本次未发现与该正文冲突的已上线移动新版证据。

完整引用现有 `ol:wenji`、`ol:tunjiang`，不新增技能定义、运行时或重复人物快照检查。已有 `src/Characters/liu-qi/ol/Skills/ol-liu-qi.rules.json` 分别调用 `receiveWenjiGiftAndArmTurnName` 与 `drawForUnskippedUntargetedPlay`；前者保留原生HE交牌者私有付款、真实交牌完成与按实际牌名发行的回合许可，后者沿实际出牌阶段经过和指定其他角色的用牌事实判断，不将结束阶段外随意的用牌计入该条件。

不能串用[十周年当前刘琦](https://x.sanguosha.com/hero/527.html)的“同类型”不可响应及省略跳过限制的屯江，也不能套用移动人物页的国战栏：国战问计按势力分支，获得原实体的距离/次数许可与返还其他牌规则均不属于这里的普通身份。

正式经典画像为[官网42300](https://www.sanguosha.cn/storage/uploads/images/skins/42300.jpg)，由官网256原始HTML皮肤列表首图确认，第二图42301。本地素材为 `src/CardGame.Wpf/Assets/official-mobile-liu-qi.jpg`，已随本批接入素材目录与SP图库。
