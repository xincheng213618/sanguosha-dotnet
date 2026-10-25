# 阚泽（移动版）

核对日期：2026-10-26。独立移动身份 `mobile:kan-ze`，吴、男性、3体力。

采用[移动版官网武将212](https://www.sanguosha.cn/pc/hero-detail-212.html)的普通身份技能。下书在出牌阶段开始交出全部手牌，受赠者选择展示任意张，阚泽在展示与未展示部分之间择一获得；完整复用 `ol:xiashu` 的原生交牌、受赠者私有选择和领取子结算。

官网宽释明确只防止指定对象在下回合开始前“下一次”超过一点的伤害。现有 OL `ol:kuanshi` 没有该次数字样且保持重复防止，因此移动版使用独立 `mobile:kuanshi`：沿同一实际结束阶段、已定值原生伤害尝试、保护记录和下回合跳摸债务；观察到首次原生防止事实时仅消费对应移动版记录，不改 OL。小于等于一点的伤害不消费记录。死亡和下回合开始到期继续走既有协议，没有新增 pending 或旁路状态。

`MobileKanZeChecks.OnlyFirstHeavyDamageConsumesProtectionAndKeepsDrawDebtCold` 对比移动和 OL 版本：一点实际受伤不耗保护，首次两点被防，第二次两点仅移动版实际受伤，保护消费后 Checkpoint/Replay 保留一次跳摸债务并在下一回合真实跳摸。下书继承原 `KanZeChecks` 的私有选择与原生交牌覆盖。

画像采用官网该普通人物页第一张[原图37100](https://www.sanguosha.cn/storage/uploads/images/skins/37100.jpg)，绑定 `portraitKey=mobile-kan-ze`。素材下载、目录绑定及图鉴整合由本批统一完成。
