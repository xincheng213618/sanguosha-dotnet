# 诸葛亮·移动版

> 此版本与 `classic:zhuge-liang` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，蜀、男、3体力、标准包，完整引用 `classic:guanxing`、`classic:kongcheng`。只新增人物资料，不复制技能 JSON 或运行时。

来源：[移动官网4](https://www.sanguosha.cn/pc/hero-detail-4.html)、[移动BWIKI人物37312](https://wiki.biligame.com/msgs/index.php?title=诸葛亮&oldid=37312)、[普通观星40222](https://wiki.biligame.com/msgs/index.php?title=观星/诸葛亮&oldid=40222)、[普通空城19841](https://wiki.biligame.com/msgs/index.php?title=空城/诸葛亮&oldid=19841)。通过 MediaWiki revisions API 核对原始 wikitext：人物修订时间2026-03-07、观星2026-08-27、空城2022-11-28。人物为正式标准包经典身份，开服上线、1将魂获取；没有采用国战、界、卧龙、友、谋或神诸葛亮的规则。人物渲染页显示的更新日期可能来自旧模板，以上以修订API时间和明确修订号为准。

普通观星在准备阶段可观看牌堆顶至多五张，人数限制是存活角色数，可以任意分配并排列至牌堆顶和牌堆底。官网旧文的“全场角色数”由当前移动技能修订明确为存活角色数。现有 `classic:guanxing` 在 `turnStartBeforeNormalFlow` 使用 `reorderTopCards`，`amount:5`、`numberExpression:livingPlayerCount`；`BeginProgramTopReorder` 先按实际可用牌量限制，再取存活角色人数的最小值，正常足量牌堆的观看量正是 `min(5,存活人数)`。牌堆顶和底的选择顺序保存在原 `TopReorder` 帧，原实体仅在完成排序时提交；顶区可以选零张，也可以全部留顶。没有界版人数不足四改看三张的算法，也没有全置底后再于结束阶段观星的额外收益。

空城直接引用 `classic:kongcheng`：没有手牌时禁止成为杀或决斗的目标。其 `prohibitTarget` policy 的完整旧杀族与 `duel` 沿 `CardPolicies` 调用 `ProgramCardKindMatch.Contains`，实际包含普通杀、火杀、雷杀和冰杀；不会因声明保留历史三个杀成员而漏掉冰杀。判断读取被选角色当前手牌数，限制作用于原生选目标资格，不是在决斗已经开始、打出最后一张杀后重新取消原目标。没有采用国战版回合外所得牌暂存武将牌的额外机制。

以上均为现有完整能力引用；沿用共享加载、注册、牌堆排序和禁止目标验证，不增加重复人物快照或测试夹具。统一编译及定向验证由主代理执行。

普通经典图：[400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/400.jpg)，为官网4首图；第二张40100为界版，不采用。`portraitKey` 为 `mobile-zhuge-liang`，图鉴分组 `standard`，人物/将池85699。
