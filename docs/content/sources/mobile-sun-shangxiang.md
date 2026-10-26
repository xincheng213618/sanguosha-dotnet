# 普通移动版孙尚香

核查日期：2026-10-26。`mobile:sun-shangxiang` 共用 `character:sun-shangxiang`，吴、女、3体力，标准包；普通身份与界、SP、谋版分开。

## 来源与身份

- [移动官网武将25](https://www.sanguosha.cn/index.php/pc/hero-detail-25.html)：官方普通孙尚香。页面源码中的隐藏技能正文同时列出普通结姻和枭姬；本次在内存直接读取正文，结姻每出牌阶段限一次，以两张手牌为费用，令本人及一名已受伤男性各回复一点；枭姬按装备区每一张牌的失去产生两张摸牌。
- [当前移动百科人物34811](https://wiki.biligame.com/msgs/index.php?title=孙尚香&oldid=34811)：普通身份开服上线，游戏等级35级取得，吴女3、标准包。页面还列国战和1V1，不能把国战每次装备丢失或1V1可选回复并入身份版枭姬。
- [普通结姻20057](https://wiki.biligame.com/msgs/index.php?title=结姻/孙尚香&oldid=20057)、[普通枭姬20044](https://wiki.biligame.com/msgs/index.php?title=枭姬/孙尚香&oldid=20044)：正文及当前页尾修订标识已读取，均显示2022-11-30编辑；人物页为2025-10-21编辑。对技能的`action=raw`及history直连被站点返回567或浏览工具不可访问，故不把HTML正文读取冒称为成功取得原始wiki模板。

## 结姻：沿既有机制独立配置阶段次数

`classic:jieyin` 实际登记于 `classic-active-cutover`，使用两张手牌，选择已受伤的其他男性角色，按原生弃牌、本人与目标各回复一点的顺序结算。但其旧定义是 `usesPerTurn:1`，没有阶段预算，并带历史经典身份入口条件，不能据同名直接声称符合当前普通移动版的每个出牌阶段一次。

独立 `mobile:jieyin` 保留既有三节点和手牌费用、目标资格，明确 `usesPerTurn:null`、`usesPerPhase:1`。激活沿正常原生Play阶段入口，不另设整回合或旧模式名称限制。孙尚香本人是女性，`otherWoundedMale` 对普通身份正好对应可选已受伤男性；本人无需先受伤，满体力时本人的回复无额外收益，目标仍按原生回复结算。

两张材料先真实弃置，后续费用移动及回复子窗继续留在原程序帧，沿原生输入验证、实际阶段配额与typed返回，不重新支付费用，不建立人物pending或新操作码。额外真实出牌阶段可以重新发动，原阶段内不能重复。

## 枭姬：完整引用既有能力

复用 `classic:xiaoji`（当前登记的 `classic-card-movement-skills` revision2）：装备来源区域、`cardsMoved`、`movementOccurrence:perCard`、可选Draw2。替换装备、其他角色取走或技能费用转移都沿真实装备离区事实处理；一次丢失多张装备按每张各一次，不错误套用国战的perBatch，也没有1V1回复分支。

本次只是既有能力的新内容组合，不增加重复的人物定义快照或运行时。既有 `MobileChenZhenChecks.SameColorHandPaymentSharedPhaseQuotaAndReplay` 通过当先提供同回合两个真实Play阶段，覆盖阶段配额更新；`ProgramActivationLimitChecks.MixedZonesAtomicityAndReplay` 覆盖真实费用与限次原子提交；`CardMovementProgramChecks.AtomicBatchRunsPerCardAndPerBatchAndReplays` 及 `BoundarySunShangxiangChecks.EquipmentAndReplacement` 覆盖逐牌装备离区、真实枭姬与子返回。没有为此重复整套隐私/冷恢复矩阵；本次未自行构建或运行测试。

## 素材与接线

普通经典形象为[官网2500.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2500.jpg)，官网25中第二张，首图40300为界版。已在本次官方HTML内确认URL，HEAD实际200/image/jpeg/262153字节。建议 `portraitKey=mobile-sun-shangxiang`，gallery `standard`。技能注册85200、人物及当前将池85299。
