# 朱桓（移动版）

核对日期：2026-10-25。普通移动身份 `mobile:zhu-huan`，吴、男性、4体力，奋励、平寇。现有经典OL朱桓仅为诱敌，不能整人复用；移动界朱桓另有平寇装备奖励，也不能整人混用。

- [移动官网武将181](https://www.sanguosha.cn/index.php/pc/hero-detail-181.html)：核对奋励及隐藏的平寇全文。奋励按手牌、体力、非空装备数量分别达到全场最多，可跳过对应摸牌、出牌、弃牌阶段；最多允许并列。普通平寇按本回合实际跳过的阶段数，选择至多相同数量的其他角色，各造成一点伤害，没有从牌堆获取装备。
- [移动版 BWIKI 朱桓](https://wiki.biligame.com/msgs/%E6%9C%B1%E6%A1%93)，更新于2026-01-24：普通身份与官网相同；诱敌被标为历史版本，国战条目另外列出，未将其国战限时身份移作本次普通身份。
- [官方2023-05-22界朱桓公告1525](https://www.sanguosha.cn/pc/news-detail-1525.html)：该公告明确针对界朱桓，平寇有额外随机装备，修改的体力为3/4到4/4。本次不把该界版附加收益混入普通朱桓。

奋励完全引用 `boundary:fenli-mobile`。它以实际开始的阶段、并列最大和装备区非空为条件，并通过原生摸牌替代及跳阶段机制结算；不以预先点选技能次数代替真正跳过的阶段。

普通平寇为独立 `mobile:pingkou`，沿用已有 `BoundaryZhuHuanPhaseSkippedEvent` 的实际阶段事实。原生实际回合结束边界统一在 `AfterTurnEnded`，实际结束帧与回合号冻结在原 `ProgramSkillFrame.MobileZhuHuanPingkou`，配额包含其他效果真正跳过的阶段。选择至多X名不同的其他存活角色后封存目标，并按实际行动顺序逐人调用已有原生伤害流程；每人的伤害、濒死与救援孩子完整返回后，原帧才向下一人前进。来源失去或持有者死亡后，已封存发行的目标仍由原帧续接。没有取得装备节点、第二个pending、伤害编号字典或全局规则版本。

新增入口 `MobileZhuHuanChecks.ActualSkipsSequentialNativeDamageAndDyingColdWithoutEquipment`：固定开局真实跳过三个阶段、反序选择两人、第一目标伤害后的真实濒死孩子cold和原生桃救援、第二目标随后受伤。牌堆保留真实装备库存，验证完整结算只造成各一点，不领取界版装备奖励，冷恢复不重伤第一人。奋励其他资格及外部实际跳阶段沿已有 `Mobile boundary Zhu Huan: external actual skip and empty equipment qualification`，不扩展种子搜索或组合矩阵。

立绘为[官网原画32800](https://www.sanguosha.cn/storage/uploads/images/skins/32800.jpg)，530×750，原图保存为 `official-mobile-zhu-huan.jpg`，未裁剪或重采样。SHA-256：`4d361cc22b73648e7e9a8b6b9b3a079adccd65da5327d69dd598eb45dbcecb94`。
