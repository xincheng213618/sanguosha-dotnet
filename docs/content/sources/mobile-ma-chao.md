# 马超（普通移动版）

核对日期：2026-10-26。`mobile:ma-chao`，蜀、男、4体力，标准包；铁骑83700，武将及当前池83799。

- [移动官网武将6](https://www.sanguosha.cn/pc/hero-detail-6.html)。可见正文列出马术的全距离减1；原始HTML铁骑标签页的隐藏正文明确是在使用杀指定一个目标后，可判定，红色令该角色不能使用闪响应此杀。隐藏标签不是未公开规则。
- [移动BWIKI当前人物](https://wiki.biligame.com/msgs/马超)，[固定修订34840](https://wiki.biligame.com/msgs/index.php?title=马超&oldid=34840)，标注2025-10-23更新；采用身份“经典”栏的开服标准武将。其问答明确方天画戟指定3人可发动3次，红色铁骑后目标不能发动八卦阵；马术对顺手牵羊等需要距离的牌同样有效。
- [马术固定修订6692](https://wiki.biligame.com/msgs/index.php?title=马术/马超&oldid=6692)、[铁骑固定修订19866](https://wiki.biligame.com/msgs/index.php?title=铁骑/马超&oldid=19866)。铁骑经典正文与官网一致；不采用同页的谋马超正文、界版压制非锁定技及弃同花色牌分支。
- 官方经典画像：[600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/600.jpg)，`portraitKey: mobile-ma-chao`，图鉴 `standard`。

## 实现与复用边界

马术直接引用 `classic:mashu`，保留对所有需要距离的用牌生效的出距离减1。既有 `classic:tieqi` 实际窗口为 `slashBeforeResponse`，晚于指定目标后；因此普通移动铁骑独立定义为 `mobile:tieqi`，不把文字相同当作实际时序等价。

移动铁骑位于原生 `cardUseTargetsFinalized` 的实际各目标候选，普通、火、雷、冰杀均可触发。每名实际目标独立选择发动或放弃，无额外次数上限；全部目标的指定后判定先于原杀各目标的响应和伤害。改判、判定子窗及实体清理复用原生 `startJudgment`，红色判断读取冻结的生效判定结果，判定实体即使已被合法领取也不改变该结果。

扩展既有 `prohibitCurrentResponse`，仅增正式指定后、实际行动者、完整杀族的合法窗口。该分支精确校验原行动nonce及被指定目标，在原 `CardUseFrame` 的当前攻击及 `PreparedTargetAttacks` 内更新该目标的禁闪状态和技能名。方天等多目标杀后续仍恢复这些原生攻击状态，不能把甲的红色结果串给乙，也不因黑色或放弃而清除其他技能已有的禁闪。原经典响应前分支及所有历史定义保持不变，没有另造响应流程、待决对象或按用牌ID保存的旁路状态。
