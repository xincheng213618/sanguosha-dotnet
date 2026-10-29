# 经验传承2474与百战老兵

来源为官方 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的 `hd_roguelike.sgs`、`cha_spell.sgs` 与武将资料。固定缓存外层 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；本批采用的解码成员：

| 成员 | SHA256 |
| --- | --- |
| hd_roguelike.sgs | 97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72 |
| cha_spell.sgs | e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219 |

`fightID=2474`、`Ggroup=2474`，战斗名称为经验传承；唯一 `General.a=2474` 行的角色 `b=4861`，名称 `c=百战老兵`。角色表4861的名称是士兵（`d=ShiBing61`），并非刘谌。原始体力/上限为4/4，通常摸牌2、随机初始手牌4、额外出杀0、护甲0、护甲回复0、护甲上限5、额外伤害0，没有固定牌或装备。`NewbieGroup=22474` 是另一战斗配置，不作为2474的隐藏成员追加。

五档技能按来源字段累计，不用高档技能替换低档技能：

| 源难度 | 原始字段与技能 | 本地复用 |
| --- | --- | --- |
| 1—5 | `v=736` 战绝 | `classic:zhanjue` |
| 6—10 | `getspell_ZD=59` 连营 | 以上加 `classic:lianying` |
| 11—15 | `getspell_KN=940` 龙胆 | 以上加 `boundary:longdan` |
| 16—20 | `getspell_EM=878` 武圣 | 以上加 `boundary:wusheng` |
| 21—25 | `getspell_LY=887` 再起 | 以上加 `shanhe:s5-zaiqi-887` |

736允许将所有手牌当决斗使用，结束后本人和因此受伤的角色各摸一张牌；本阶段本人以战绝摸过至少两张牌后失效。59是失去最后手牌后可摸一张牌。940同时包括杀闪互换与酒桃互换。878包括红牌当杀，以及方片杀无距离限制，因而复用界武圣。

887（`ZaiQi2019`）原文从亮出一张起算，每损失一点体力多亮一张；回复其中红桃牌数量的体力并取得其余牌。它与已有山河图源887完全一致，直接复用其“已损失体力+1”亮牌、红桃过滤、原生回复、红桃入弃牌堆、其余实体牌入手的程序。内容工厂按需要注册既有束，不复制规则、技能身份或再起结算机制。满血不提供替代摸牌，通常摸牌按既有流程继续。

五档独立身体由 `StandardShanheSourceVeteranPackage.GetGeneralId(1..25)` 选择，角色身份统一为 `character:shanhe-source-soldier-4861`；运行时保留 `supporter` 回退键，桌面可按下列精确来源卡面及运行时ID绑定显示。来源数值成长仍由共用来源成长流程按真实难度与城市章节施加，包内只保留原始4/4身体，不叠加推荐路线的本地成长，也不借用刘谌身体或肖像。

## 原生卡面

已于2026-10-29核对普通组2474完整单人源行：`rawOrdinal=0,b=4861,c=百战老兵,d=1000129`，基础与四档追加技能736/59/940/878/887均与上述身份链相符。`character.sgs` 中4861为“士兵”、类 `ShiBing61`，无 `au/av` 重定向；皮肤来自本行明确的 `d`，不按共享载体借其他士兵图。`cha_gs_dbs_fs_skininfo.sgs/root.pveskin` 的1000129行指定 `seatpic=generalface_1000129`、`headpic=1000129`、`diagrampic=100012900`。解码皮肤表SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`，解码角色表SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`。

客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 由 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 生成[官方PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000129.png)。HTTP200、`image/png`，已目视头巾、披巾及皮甲男性老兵原卡。原生146×172 RGBA、51752字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png`；文件SHA-256为 `6b9cb640e2e4eab48ec1243605fb3cd50f08774e3e12ea063e5d4be87097ec30`，RGBA像素SHA-256为 `5fc269f63371286be1a782177400bb567153a94729f0b4bd246422e733a5dced`。未裁剪、缩放或换色。精确运行时ID依次为 `shanhe:source-veteran-2474-base`（1–5）、`shanhe:source-veteran-2474-medium`（6–10）、`shanhe:source-veteran-2474-hard`（11–15）、`shanhe:source-veteran-2474-nightmare`（16–20）、`shanhe:source-veteran-2474-hell`（21–25）。同组五档均使用本行皮肤，成员 `startChapter=1,f=0`，缺省 `g/hide/generalShow` 按源默认false/false/0，无主帅及额外成员。

本批代表性前向检查使用实际中等档士兵，连续两次将全部手牌付为原生决斗：先等待真实连营孩子返回，再继续同一决斗的响应、伤害、实体弃置和双方完成摸牌；连营的独立摸牌不计入战绝的本阶段两张阈值。887沿用既有能力，没有为各档复制静态快照或执行存档、恢复、回放、完整对局检查。

该组合的实际检查发现，原全手决斗的付款批次曾延迟到决斗结束才处理连营。本批让由全手牌普通锦囊操作实际发行的决斗复用既有 `PaymentMovementReceipt` 和原生付款返回：核对原发动者、技能实例、全部原手牌和实际移动批次后，先在同一决斗帧上处理失牌孩子，再沿 `ResumePaidCardUseFrameId` 继续原决斗。不重新发动决斗，不重复付款，不扩大到一般普通锦囊的付款规则。
