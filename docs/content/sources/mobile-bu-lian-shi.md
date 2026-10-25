# 步练师（移动版）

核对日期：2026-10-25。普通移动身份 `mobile:bu-lian-shi`，吴、女性、3体力，安恤、追忆。当前规则与已有经典OL安恤、移动界安恤均有实际区别。

- [移动官网武将117](https://www.sanguosha.cn/index.php/pc/hero-detail-117.html)：核对HTML全部隐藏技能正文。安恤先选两名手牌数不同的其他角色，由手牌较多者自己交出一张手牌；结算后双方手牌相等，持有者摸一张或回复一点体力。追忆在自己死亡时，可以令一名其他角色摸三张并回复一点，排除实际杀死自己的角色。
- [官方2015-12-01技能调整公告98](https://www.sanguosha.cn/pc/news-detail-98.html)：明确将旧的“低手牌方抽取、展示、非黑桃奖励”安恤改为现在的“高牌方交牌、相等后摸牌或回复”。因此不能别名 `classic:anxu`。
- [移动版 BWIKI 步练师](https://wiki.biligame.com/msgs/%E6%AD%A5%E7%BB%83%E5%B8%88)，更新于2025-10-21，及[安恤技能子页](https://wiki.biligame.com/msgs/%E5%AE%89%E6%81%A4/%E6%AD%A5%E7%BB%83%E5%B8%88)，更新于2023-01-26：当前正文与公告一致，旧版被单列为历史版本。本次没有引用界步练师的装备区获得、手牌较少者追加摸牌，或OL界的双人各取一牌及固定受益者规则。

追忆完整复用 `classic:zhuiyi`：原死亡窗口、排除实际伤害来源的合法受益者、原生摸三张后回复一点均沿既有实现。

安恤为 `mobile:anxu`。每实际出牌阶段限一次，先用已有不等手牌对象选择，冻结原低牌方和高牌方；使用已有 `SelectAndMoveOwnedCard`，选择者和原牌主均为高牌方，受赠者为低牌方，只允许真实手牌，不展示这张牌，并接入原生Give语义。交牌及其实际获得孩子完全返回后，薄操作 `PrepareMobileBuLianShiAnxu` 比较原两名角色此时的真实手牌数，以描述符声明的计算结果资源在原帧冻结 `equal` 或 `unequal`，并发行仅含标量的比较事实；后续只在冻结结果相等时开放奖励。计算结果的精确生产者校验核对原帧、持有者、技能实例、内容哈希、原两名对象、真实赠牌付款及唯一比较事实，不将他人的赠牌伪装为持有者拥有的稳定牌集。真实最后入手移动在原帧发行标量交付事实，原giver、recipient、绑定实体、native batch和移动序号均可由实际账本核验；交牌孩子中来源失去后，原付款返回仍继续结清这次比较及奖励，不再次给牌。后续可选摸一张或回复一点全部使用已有DSL，受伤时才提供回复选项；没有另建pending、抽牌或恢复实现。

新增入口 `MobileBuLianShiChecks.DonorChoosesActualGiftAndPostChildEqualityCold`。固定1/3手牌场景验证高牌方真实选择黑桃手牌、实际交牌孩子cold、2/2相等后持有者回复一点及本阶段限次；同一入口另以1/4变成2/3验证不相等没有奖励。它保留规则差异，不用旧版非黑桃条件替代。追忆沿既有 `Mobile boundary Bu Lian Shi: equipment children equal actual death benefit cold` 的死亡收益原生流程及共享恢复检查，不复制人物定义快照。

立绘为[官网原画51500](https://www.sanguosha.cn/storage/uploads/images/skins/51500.jpg)，530×750，原图保存为 `official-mobile-bu-lian-shi.jpg`，未裁剪或重采样。SHA-256：`b930017bf147c64faff5d5a1d46e15a6fd266aae8557f62dde96a6774fc4a568`。
