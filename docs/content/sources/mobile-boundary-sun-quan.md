# 移动版界孙权

核对日期：2026-10-25。独立身份 `boundary:sun-quan-mobile`，吴、男性、四体力，画像键 `boundary-sun-quan-mobile`，技能为 `boundary:zhiheng`、`boundary:jiuyuan-mobile`。

- [移动版官网武将详情235](https://www.sanguosha.cn/index.php/pc/hero-detail-235.html)：完整制衡及救援。制衡每出牌阶段一次，弃置所选本人牌后摸等量；若实际选择包含全部手牌，额外摸一。救援只针对其他吴角色对自己使用桃，比较为该角色体力严格高于孙权，然后由该角色选择改为孙权回复一，再由该角色摸一。
- [移动版Wiki界孙权](https://wiki.biligame.com/msgs/%E7%95%8C%E5%AD%99%E6%9D%83)与[移动版救援专页](https://wiki.biligame.com/msgs/%E6%95%91%E6%8F%B4/%E7%95%8C%E5%AD%99%E6%9D%83)：身份版与当前官网一致。已检索同站官方改版公告，未找到更晚公告取代该完整定义；不将搜索到的其他武将改版、国战或谋孙权条目混入。
- [OL救援专页](https://wiki.biligame.com/sgsol/%E6%95%91%E6%8F%B4)仅作版本区别：OL界版的本人回合内任意回复、体力不小于孙权，与本移动版“自身桃、严格大于、无本人回合限制”不同，不能直接授予原 `boundary:jiuyuan`。

制衡完整复用现有 `boundary:zhiheng` 的实际HE选择、实体弃置、原摸牌数量、全手牌奖励和实际阶段次数，不复制节点或夹具。

救援沿已有原生 `RecoveryAttempt`、`RecoveryReplacementFrame` 与已支付返回合同，只增加本版本的资格规则。现有回复替换政策作为相同原生能力容器使用，本版本先排除其旧的本人回合与宽体力比较，再从原生实际桃使用、真实用牌人等于回复目标、吴势力、主公来源及技能实例中建立严格候选；不把普通技能回复、白银狮子回复、桃园结义或他人对该角色使用桃纳入。未人为增加“其本人回合”的限制。

原桃的真实实体付款、回复替换选择、孙权回复的HP子窗、随后提供者摸牌及其移动孩子全部留在原回复与用牌帧。选择已支付后失去原技能不取消已经发行的摸牌，也不改为给孙权摸牌。候选、付款事实与真实移动凭据继续由共享原生恢复证明验证，不新增pending、使用ID旁路、保存版本或枚举。

新增一条 `BoundarySunQuanMobileChecks.SelfPeachStrictComparisonPaidRecoveryAndRewardCold`：相等体力的真实自身桃不替换，严格较高但非桃的原生回复不替换，合格自身桃则沿原回复帧令主公回复后提供者摸牌；在真正失去主公来源后的奖赏移动子窗冷恢复，验证回复、摸牌及原桃实体支付都仅一次。完全复用的制衡继续使用 `Recovery replacement Zhiheng selection and phase quota`，同批仍选择旧OL回复替换检查保留其原行为。
