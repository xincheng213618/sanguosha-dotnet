# 普通遭遇2485：曹嵩与双卫兵

## 来源与完整成员

来源为[《三国杀OL》官方配置](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)中的 `hd_roguelike.General.a=2485`，技能全文取同版 `cha_spell`，原人物取同版 `character`。本批重新解码并核对既有冻结官配，与普通遭遇2492使用同一版缓存；没有宣称重新下载线上配置。

| 核对对象 | SHA-256 |
| --- | --- |
| Config.sgs | `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6` |
| client.sgs | `96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31` |
| hd_roguelike.sgs | `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72` |
| cha_spell.sgs | `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219` |
| character.sgs | `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de` |

完整保留原三员顺序，不能只以曹嵩代替整组。

| 顺序 | 源身体 | 原显示名 | 体力/上限 | 基础摸牌/起手 | 角色身份 |
| --- | --- | --- | --- | --- | --- |
| 1 | 438 | 曹嵩 | 6/6 | 2/4 | `character:cao-song` |
| 2 | 4861 | 卫兵 | 2/2 | 2/4 | `character:shanhe-source-soldier-4861` |
| 3 | 4862 | 卫兵 | 2/2 | 2/4 | `character:shanhe-source-soldier-4862` |

曹嵩保留原行 `g=1` 的首领标记；这是普通遭遇内的首领，不代表二阶段。三员均无固定装备、隐藏角色、变身跳转或额外 `otherad`。不导入独立的新手组22485，也不把 `pvpspell=3189` 当作普通战斗技能。曹嵩原人物的礼赂9017没有出现在本组技能行，因此不附带。

曹嵩采用已有 `wei` 身份与男性设定。冻结Character438没有头像替代或版本门槛，原生席位图按默认ItemID链得到 `generalface_438.png`；此族三个精确来源ID使用该原卡。既有 `ol-cao-song` 的750×950大图保留给其他角色，不将其当成146×172原生席位图的同字节复本。双卫兵保留各自原身体身份和本地普通士兵的 `qun`/男性约定；运行时保留 `supporter` 回退键，桌面原图按两条独立源行共同指定的PVE皮肤1000215复用既有原始PNG，覆盖两卫兵各自基础／困难四个精确ID，不合并两席或借用别人的身体。

## 1—25档累计配置

| 难度 | 曹嵩技能 | 两名卫兵各自技能 | 两名卫兵各自起手 |
| --- | --- | --- | --- |
| 1—10 | 翊正9018 | 无 | 4 |
| 11—15 | 翊正9018、威重239 | 穿云665 | 4 |
| 16—20 | 翊正9018、威重239 | 穿云665 | 5 |
| 21—25 | 翊正9018、威重239、龙威1602 | 穿云665 | 6 |

卫兵原行 `att_EM=cardnum,1` 与 `att_LY=cardnum,1` 在16档、21档分别累计，交给地图来源属性层执行，不改基础身体或把最终起手数硬写到所有难度。曹嵩始终6/6、D2H4。

## 技能版本与实效

**翊正9018**：结束阶段选择一名其他角色，直到自己的下回合开始，其造成伤害或回复体力时，若体力上限小于曹嵩，则曹嵩减少1点上限，此伤害或回复值+1。精确复用 `ol:yizheng-cao-song`，沿用原结束窗口、实际上限支付与伤害/回复父帧；没有曹嵩身体或势力锁定，也不因此带入礼赂。

**威重239**：锁定技，体力上限变化时摸一张，若手牌数全场最少则改摸两张，包含并列最少。精确复用 `shanhe:source-weizhong-239`；包依赖既有普通马日磾包，使该定义只注册一次。

**穿云665**：结束阶段，可以对一名体力值大于自己的敌方角色造成1点伤害。新增独立源定义 `shanhe:source-chuanyun-665`，以公开队伍关系和实际体力筛选目标，再走通用真实伤害节点。相等体力的敌人、更高体力的友方均不是合法目标。同版客户端 `ChuanYun` 使用敌方选择器，并明确比较 `target.Hp > owner.Hp`，与原文一致。

**龙威1602**：友方角色进入濒死时，可以减少1点体力上限，令其回复至1点体力。新增独立源定义 `shanhe:source-longwei-1602`，只绑定当前原生濒死进入帧的友方对象；友方包含技能持有者本人。执行顺序为选择实际濒死者、通用上限减少1、通用回复至1。上限变化触发威重及摸牌孩子后，先回到同一龙威帧，再恢复原对象；负体力时按回复至1计算回复量，不简化为回复1点。

本地沿用通用上限变更的最低1点规则。龙威要求持有者上限大于1，确保实际减少1点，不能在上限1时无费用救援。同版客户端 `LongWei` 的发动准入由服务端下发，没有提供上限归零的确证；此处明确是本地支付边界，不冒充官服零上限处理结论。

新增目标域仅负责资格；伤害、上限变化和回复均复用既有执行节点，状态由原程序帧与原濒死帧持有，不增加并行待处理对象、技能schema或规则版本。

## 实际城池与包接口

`UniversalGroup` 将2485放入14、16、18、19、20、21、22、26池。连接赛季5的 `Level.universalfight` 与 `Chapter.location` 后，对应第二章8个地区、56城：

- 5011—5017：大谷关。
- 5031—5037：函谷关。
- 5051—5057：轘辕关。
- 5061—5067：伊阙关。
- 5071—5077：颍川郡。
- 5081—5087：孟津。
- 5091—5097：小平津。
- 5131—5137：荥阳。

`startChapter=1` 是原行字段，实际投放仍按上述城市连接结果。`Level.DDDDID=5` 是赛季字段，不误作地区编号。

包为 `StandardShanheSourceCaoSongTrioPackage`，ID前缀 `shanhe:source-caosong-trio-2485-`。`GetRosterGeneralIds(difficulty)` 按曹嵩、4861、4862返回三员；分别提供 `GetCaoSongGeneralId`、`GetFirstGuardGeneralId`、`GetSecondGuardGeneralId`。共7个技能档身份；起手属性由地图累计，故16档不额外复制同技能身份。

新鲜聚焦检查 `Source665EnemyHpAnd1602PaidMaximumChildBeforeNegativeHpRescue` 验证严格敌方体力选择、敌方濒死排除、实际威重摸牌孩子返回、负体力恢复至1及上限支付最低边界。只从新建游戏和实际命令驱动，不运行存档恢复、回放或完整对局检查。纯复用9018/239由原共享机制检查覆盖，不复制角色行为快照。


## 2026-10-29 原生席位图与预览身份

完整源组逐行保留；`hide` 缺省false、`startChapter=1`、`generalShow` 缺省0。下表按从0开始的源行顺序记录真实 `g`，未列出的 `g` 按0处理；不从首席、同名、通用载体或将领称呼推定主帅。身份只用于预览布局，不改变关卡类型、技能、数值、抽选或内容版本。

| 源行与成员 | g | 原图链 | PNG | 字节 | SHA256 |
| --- | --- | --- | --- | --- | --- |
| raw0 曹嵩（438） | 1 | 无General.d → Character438默认ItemID | [官方146×172原图](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_438.png) | 54075 | `02361373c7bfe126d0ea8f3dc49686c15ca76dd108318aa4b6910cdbcbf16be4` |
| raw1 卫兵（4861） | 0 | General.d=1000215 → root.pveskin.seatpic | [官方146×172原图](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000215.png) | 51324 | `b7cdae24bfa47ff260127855116a095cec1d768cddcbb58a9dbbfde4d33838fd` |
| raw2 卫兵（4862） | 0 | General.d=1000215 → root.pveskin.seatpic | [官方146×172原图](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000215.png) | 51324 | `b7cdae24bfa47ff260127855116a095cec1d768cddcbb58a9dbbfde4d33838fd` |

所有表内PNG均取得HTTP200、image/png响应并完成目视、尺寸、原字节SHA及RGBA像素核对；直接使用原始响应字节，不裁剪、缩放、重绘或重新编码。多人组的重复成员仍占独立席位；同一PNG的复用必须有各条源行的独立皮肤证据。当前完整来源配方的25个难度值均已只读检查，体力成长和固定装备不改变名单或角色顺序。原图、精确运行时绑定与身份布局证据集中记录在[山河NPC美术来源](../shanhe-npc-art-sources.json)及[敌将布局来源](../shanhe-encounter-layout-sources.json)。

Character438／634的解码字节SHA256与既有冻结记录一致（`92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`），两个角色均无 `au`／`av`／`needVersion`；按已核对的 `needVersion||0`、LOG_VERSION40303及默认ItemID链解析原生席位图。此次公开配置和客户端外包哈希已经不同，不将当前外包冒充旧冻结包。旧750×950大图与原生146×172卡图像素不相同，仅此组精确来源ID使用原生图，其他普通武将默认图保留。
