# 移动版界庞德

核对日期：2026-10-25。独立身份 `boundary:pang-de-mobile`，群、男性、四体力，画像键 `boundary-pang-de-mobile`。

- [移动版官网武将详情267](https://www.sanguosha.cn/index.php/pc/hero-detail-267.html)：马术计算与其他角色距离减一。鞬出在使用杀指定其他目标后，可以弃置目标一张牌；弃置装备则该目标不能使用闪响应本杀，弃置非装备则目标获得本杀。
- [移动版Wiki界庞德](https://wiki.biligame.com/msgs/%E7%95%8C%E5%BA%9E%E5%BE%B7)，2025-12-30更新：当前身份规则采用同一装备/非装备分类，装备分支写作不能响应此杀。这里原生杀所需的闪回应直接被阻止，不能套成支付闪但不抵消。
- [2020-06移动版官方攻略46](https://www.sanguosha.cn/pc/guide-info-46.html)交叉说明本版马术、装备弃置与破坏手牌。核查同站后续公告未发现将本身份规则替换为OL基本/非基本分类及追加杀次数的修订；国战和SP版不混入。

马术完整复用 `classic:mashu`。鞬出登记 `boundary:jianchu-mobile`，沿实际杀的最终目标窗口选择原生HE区域实体弃牌。非装备包括基本牌和锦囊；二者均令目标获得仍可取得的原杀实体，之后原杀继续结算并允许普通闪响应。没有实体的虚拟杀不能凭空造出一张实体杀，多实体杀继续沿共用完整物理材料认领合同。

装备分支复用原杀上的 `FinalTargetSlashReceipts`，由薄版本谓词将本移动技能的原目标收据接入已有禁止回应入口；直接跳过该目标的闪请求。不会禁止原杀其他目标，不需要继续拥有鞬出来维持已发行效果，也不增加本回合可用杀次数。

弃牌付款采用原 `ProgramSkillFrame` 的命名牌绑定及全scalar原生付款事实，冻结该技能帧、实际用牌帧、动作、原目标与真正移动序号。孩子返回按同一已付事实续接，即使孩子内失去技能也不重复支付或丢失非装备分支的原杀认领。真正的实体认领、杀的后续伤害、来源与目标存活检查仍由原共用机制执行，未建立新pending或用牌ID旁路仓库。

最少新检查入口 `BoundaryPangDeMobileChecks.NonEquipmentTrickClaimsOriginalSlashAndEquipmentForbidsDodgeAfterPaidSourceLossCold`：固定实际锦囊弃牌与装备弃牌两条短前缀，各在原付款孩子冷恢复并实际失去来源后继续，分别确认原杀实体认领/真实闪抵消，以及装备分支完全不请求或消耗闪。两条均确认没有OL额外杀次数。继续保留 `Current Slash entity claim basic payment original damage and multiple materials` 覆盖旧OL实体认领合同及多材料原杀，不削弱旧版断言。

官方原始立绘取自官网267的首张[43400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/43400.jpg)，530×750 JPEG，原图保存，没有裁切、缩放或重绘。
