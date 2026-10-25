# 移动版界马超

核对日期：2026-10-25。独立身份 `boundary:ma-chao-mobile`，蜀、男性、四体力，画像键 `boundary-ma-chao-mobile`。

[移动官网156完整正文](https://www.sanguosha.cn/index.php/pc/hero-detail-156.html)和[移动铁骑专页](https://wiki.biligame.com/msgs/%E9%93%81%E9%AA%91/%E7%95%8C%E9%A9%AC%E8%B6%85)确认马术距离减一；铁骑在实际杀目标确定后可压制目标非锁定技至回合结束，再进行判定，除非该目标实际弃置一张与最终判定同花色的牌，否则不能使用闪响应本次杀。当前专页2025-10-23更新。检索同站后续公告未找到身份版新修订；不混入谋马超的谋弈或国战的明置单将压制规则。

马术完整复用 `classic:mashu`。铁骑薄配置 `boundary:tieqi-mobile` 继续使用既有最终目标、技能压制、原生判定及同花色HE弃牌操作，真实判定替换、付款移动孩子和恢复均留在原杀/技能帧。

仓库原OL铁骑允许支付真实但不能抵消杀的闪；本移动版必须直接禁止闪回应。新增版本判定仅检查原杀上的 `JudgmentSuitDodgeRestriction` 与其唯一原技能结算事实的帧、实际用牌人、目标及未付款状态，通过已有原生不能回应入口跳过闪。实际付款后原机制移除该限制，正常闪回应恢复；不新增pending、平行限制仓库或使用ID旁路。

新增最少行为检查复用 `BoundaryMaChaoChecks` 原夹具：固定牌堆，不搜种子，在同花色付款选择处实际冷恢复后继续，验证拒付不请求或消耗闪，付款则只弃一次且真实闪可以抵消。保留原OL的 `boundary Ma Chao Tieqi Paoxiao exact cancellation integration` 回归及全部断言，防止把两版行为混同。
