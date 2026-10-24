# 移动版界公孙瓒

核对日期：2026-10-25。人物 `boundary:gongsun-zan-mobile`；群势力、男性、4体力，技能 `boundary:yicong-mobile`、`boundary:qiaomeng-mobile`。独立于本仓库 OL 界版及谋版。

## 来源

- [移动版官网：界·公孙瓒](https://www.sanguosha.cn/pc/hero-detail-381.html)：当前两项技能正文完整。义从按照当前体力减一、已损失体力减一分别改变两个方向的距离。趫猛明确限定黑色杀、受到实际伤害的角色、其装备区，以及装备先进入弃牌堆后再取得坐骑。
- [三国杀移动版 Wiki：界公孙瓒](https://wiki.biligame.com/msgs/%E7%95%8C%E5%85%AC%E5%AD%99%E7%93%92)：同版文字交叉核准；规则以官网当前完整正文为准。

## 规则和实现

义从使用原生距离查询的基础项扩展，出向贡献为 `1−当前HP`，入向贡献为 `最大HP−当前HP−1`，实时读取实际角色状态并遵守原有技能有效性。两个参数保留符号，满体力时入向为 −1，仍存活的0体力角色出向为 +1；仅整个原生距离计算最终按引擎规则取最小值1。没有把移动版公式替换成 OL 的固定 −1/低体力 +1。

趫猛复用 `DiscardDamageTargetAndClaimMount` 的原伤害窗口、原使用牌及 `ProgramDamageTargetMount` 付款记录。仅此移动版技能要求原生 Use 的最终牌名属于杀且冻结的有效颜色为黑色；无色视为杀不满足黑色。单张、转化、实体组合及元素杀均从同一真实使用牌读取有效属性，响应不视为使用。混合花色的全黑组合仍可满足黑色，不能因为组合无单一花色漏掉它。伤害源仍须是当前真实使用者，伤害确实结算，不能以失去体力或无来源技能伤害代替。

本版在资格收集、公开选项、付款前再验证及已付款恢复中都只允许原装备区。其余既有调用保留原有 HEJ 范围和条件。装备首先经过原生弃置移动、装备移除处理及其触发子窗；随后仅当同一张原实体确实进入且仍在弃牌堆、印刷类别属于坐骑、获得者仍存活时取得。坐骑进入手牌，装备替换、触发取走及临时牌离场继续服从既有移动机制。不能先复制或直接装备一张新坐骑。

付款、原伤害/牌使用标识、原装备来源和两个移动序号保留在既有母帧；冷恢复继续原弃牌/获得子窗，不重复支付。没有新增待结算侧表、版本号或并行牌实体。

## 必要行为检查

`BoundaryGongsunZanMobileChecks.LiveSignedHpDistanceIncludesFullHealthAndDying` 通过真实失去体力、回复及濒死状态核对两个方向的实时有符号公式。

`BoundaryGongsunZanMobileChecks.BlackActualSlashDiscardsOnlyEquipmentAndClaimsOriginalMountCold` 覆盖原生无色虚拟杀及红色实体杀不发动、黑色实体杀仅能支付装备、原坐骑先弃后得，以及两个已付款子窗的冷恢复。既有 `Damage target mount real HEJ privacy inputs` 继续覆盖共用付款协议与 OL 范围；不为本人物复制整个隐私、非法输入和回放矩阵。
