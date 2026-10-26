# 移动版糜竺

> 此版本与 `ol:mi-zhu` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。身份 `mobile:mi-zhu`，人物 `character:mi-zhu`，蜀、男、3 体力。

来源：[官网武将页 207](https://www.sanguosha.cn/pc/hero-detail-207.html)、[移动版 BWIKI 人物正文（oldid=26947）](https://wiki.biligame.com/msgs/index.php?title=糜竺&oldid=26947)及[资援技能页（oldid=21577）](https://wiki.biligame.com/msgs/index.php?title=资援&oldid=21577)。资援每个出牌阶段限一次，将任意张点数之和恰为 13 的手牌交给一名其他角色，然后令其回复一点体力；巨贾将体力上限同时加到初始手牌数和手牌上限。

官网资援明确写“手牌”，BWIKI 人物/技能页略写为“牌”；本身份按官方明确的手牌来源实现，没有据省略字扩大到装备区。巨贾正文由 BWIKI 补足官网 HTML 未展开的第二技能。与本库 `ol:ziyuan`、`ol:jujia` 的完整实现相同，直接复用精确点数和、真实赠牌及回复、动态体力上限修正。不使用欢乐三国杀的资援大小于 13 分支或回合开始五谷丰登版本，不添加重复夹具。肖像键 `mobile-mi-zhu`。
