# 简雍（移动版）

核对日期：2026-10-25。普通移动身份 `mobile:jian-yong`，蜀、男性、3体力，完整复用 `classic:qiaoshui`、`classic:zongshi-pindian`。

- [移动官网武将127](https://www.sanguosha.cn/index.php/pc/hero-detail-127.html)：已读取完整HTML中的巧说及隐藏纵适。巧说在出牌阶段开始时发起拼点；赢后本回合下一张基本牌或普通锦囊可以增减一个目标，未赢则本回合不能使用锦囊。纵适在自己参与的拼点结束后，赢时可取得较小点数的原拼点牌，否则可收回自己的原拼点牌。
- [移动 BWIKI 简雍](https://wiki.biligame.com/msgs/%E7%AE%80%E9%9B%8D)，页面更新于2026-02-14：纵适与官网相同，问答明确平局算没赢、可收回自己的牌，也适用于别人发起的拼点。

存在明确的来源冲突：上述 Wiki 把巧说的成功与失败寿命写作“本阶段”，而当前官网127两处均为“本回合”。检索未找到将普通移动简雍改为阶段寿命的有日期官方修订，本次采用官方当前完整正文的回合寿命，保留这项差异供后续规则更新核验。不能用界简雍385的阶段内主动拼点、私看牌堆顶等规则代替普通简雍。

现有经典巧说的原生阶段开始触发、完整拼点和本回合禁用锦囊直接复用；共用下一张牌目标调整只在本技能的下一张基本牌或普通锦囊实际使用时消耗，装备与延时锦囊不提前消耗。拼点双方真实实体、平局及来源身份由原生拼点结果保存；纵适直接使用原 `pindianClaim` policy，赢取较小者、未赢取自己，并沿原 `PindianFrame` 完成，不创建移动版副本或平行状态。

本人物为纯复用，不新增重复 fixture。沿用 `active programs suspend into Pindian and branch from the frozen result`、`Random Pindian both strategies exact RNG tied Slash claims cold replay` 以及共用目标调整、原付款返回检查。经典巧说消耗条件修复与该共享能力一起验证。

立绘为[官网当前原画53300](https://www.sanguosha.cn/storage/uploads/images/skins/53300.jpg)，530×750，保存为 `official-mobile-jian-yong.jpg`，未裁剪、重采样或重绘。SHA-256：`ef4a7f940b49396ef4a27dae9c0cb3dcb309eac229b775faea44e5f4512ba082`。

原巧说展示中“下一张牌”的缩写已同步为“下一张基本牌或普通锦囊牌”，与实际资格及消费时点一致；装备和延时锦囊不提前消费。角色声明和程序展示两处同时更新，技能ID仍完整复用。
