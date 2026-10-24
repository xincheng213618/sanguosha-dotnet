# 移动版界马岱

核对日期：2026-10-25。人物 `boundary:ma-dai-mobile`，蜀势力、男性、4体力，独立移动版界限突破版本。

## 来源与版本

- [移动版官网：界·马岱，编号116](https://www.sanguosha.cn/hero-detail-116.html)。两项技能正文完整：马术为计算与其他角色距离减一；潜袭在准备阶段摸一、弃一，再选择距离一的一名角色，令其本回合不能使用或打出与所弃牌同色的手牌。
- [移动版 BWIKI：界马岱](https://wiki.biligame.com/msgs/%E7%95%8C%E9%A9%AC%E5%B2%B1)，2026-01-10编辑。确认该版界将2012、4体力和摸弃顺序；另列有判定式原版。其当前条目将最后的“手牌”省写为“牌”，实现遵循官网明确的手牌范围，不扩为装备区等其他来源。

不采用 OL 界马岱在出牌阶段展示牌、对所有距离一角色封牌并提高展示牌伤害的规则；也不采用判定式旧潜袭。

## 完整复用范围

马术直接引用既有正式 `classic:mashu`，其内容是永久计算对其他角色的距离减一。潜袭使用独立 `boundary:qianxi-mobile` 内容 ID，整条流程均由既有 DSL 实现，无新运行时、枚举、事件或并行 pending。

准备阶段使用 `turnStartBeforeNormalFlow`，没有继承旧 `classic:qianxi` 配置中的 `classicIdentityMode` 限制，因此在允许此技能的其他正式模式中也可按实际准备阶段发动。原生摸一张完成后，由本人选择真实手牌／装备区的一张弃置，弃牌前捕获其有效花色；`awaitMovementTriggers` 等待原生付款移动及其子窗后，再计算当时距离为一的其他存活角色。没有可付牌或没有可选目标时沿既有节点的取消语义结束，不制造牌或替代目标。自身距离为零，因而不是距离一的候选。

`grantTurnHandColorRestriction` 读取原弃牌绑定的冻结花色来确定颜色，后续回复、改色条件变化或真实装备离区不重新解释已付颜色。本回合限制使用与打出相同颜色的手牌，包括作为转化材料的真实手牌；不据此禁止装备区材料或无手牌材料的原生响应，也不改变正常弃牌、交牌等操作。限制沿既有实际回合结束过期机制退出，不缩短为出牌阶段结束。

## 验证复用

本版没有新增机制，按项目约定不复制人物配置快照或完整夹具。集中验证使用既有检查：

- `boundary ma dai shown color hand ban and equipment response`：共享颜色禁手牌与装备响应边界。
- `turn card-use effects union order consume and expire by action semantics`：按动作语义检查本回合限制与到期。
- `Huan Shu Jun: Peach and gifted equipment use restoration children cold`：真实原弃牌颜色绑定、禁手牌材料及代表冷恢复。
- `Program trigger resource contracts`：冻结花色资源、原阶段上下文的加载合同。

新人物内容由现有目录自动发现、正式内容加载和图鉴注册集成检查验证，不增加全局 schema、规则 epoch 或包版本。
