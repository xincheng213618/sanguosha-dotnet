# 移动版界孙策

角色为 `boundary:sun-ce-mobile`，移动版身份规则，吴、男、4体力。核对日期：2026-10-25。

主源为[移动版官网武将295](https://www.sanguosha.cn/index.php/pc/hero-detail-295.html)，复核其完整技能正文与玩法说明；第二来源为[移动版BWIKI界孙策](https://wiki.biligame.com/msgs/%E7%95%8C%E5%AD%99%E7%AD%96)。二者一致：激昂只在使用或成为决斗、红色杀的目标后摸一；魂姿在准备阶段体力小于3时觉醒，减一点体力上限并获得英姿、英魂；制霸由其他吴势力角色在其出牌阶段向主公提出，发起者没赢时主公可获得两张拼点牌，且只有主公已觉醒才可拒绝。官网正文“体力不大于2”与BWIKI“小于3”等价。检索官方后续调整信息，未发现取代这组移动身份规则的修订。

既有 `boundary:sun-ce` 是另一套OL规则，不能整人复用：其激昂另有失去体力获得弃置杀/决斗，魂姿门槛为1且有觉醒回合结束奖励，制霸现有主动实现则由技能拥有者向吴角色发起。移动版均不采用这些效果。`classic:jiang` 的事件机制可复用，但其配置只列普通与火杀，所以独立 `boundary:jiang-mobile` 保留四个原生触发并完整列入雷杀、冰杀，无OL返牌。`boundary:hunzi-mobile` 复用实际准备窗口、一次游戏用量、原生上限变化及技能授予，获得完整 `boundary:yingzi`（额外摸牌及体力上限手限）和 `classic:yinghun`。

`boundary:zhiba-mobile` 使用已有的外部技能行动、主公资格、吴势力查询、原生出牌阶段贡献次数及原生拼点。Lord-owned原帧记住实际吴方发起者与所选手牌，吴方保持原生拼点 `SourceSeat`，主公为 `OpponentSeat`；不将对方手牌伪装为主公材料。觉醒拒绝在拼点付款前进行；接受后仍由原生拼点选择、亮出、平点处理、可选获牌和typed return完成。可选获牌保留原 `PindianFrame` 的claim游标，不另建Use-ID状态表。原帧与原拼点都可冷恢复；已发行拼点在付款孩子中失去来源不会重新付款。

最少新增Core入口为 `BoundarySunCeMobileChecks.WuInitiatesLordTieClaimColdAndAwakenedRefusal`：固定四人原生身份桌，未觉醒吴方挑战、真实同点拼点后获牌的冷恢复、下一本人准备阶段2体力觉醒、下一吴方出牌阶段原生拒绝。激昂纯配置与原四触发机制复用，不再复制旧激昂矩阵。既有拼点与技能授予检查继续覆盖公共机制；最终运行结果由批次集成验证记录。

画像取官网295默认第一张[官方48100立绘](https://www.sanguosha.cn/storage/uploads/images/skins/48100.jpg)，原JPEG、530×750，保存为 `src/CardGame.Wpf/Assets/official-boundary-sun-ce-mobile.jpg`，SHA-256为 `09f15a230615658822d17c067e6b3d60c40223295f1e761a3011e43a9a50ccc2`，未裁剪、未重绘。
