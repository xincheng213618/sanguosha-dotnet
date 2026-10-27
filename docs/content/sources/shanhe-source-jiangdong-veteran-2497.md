# 狭路相逢2497与江东老兵

来源为官方 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 与同版本 [sgsGame_a.sgs](https://web.sanguosha.com/220/h5_2/sgsGame_a.sgs)。本批只读固定公开缓存，在内存中解码，不请求游戏服务器或创建游戏会话。

| 资源 | SHA256 |
| --- | --- |
| Config.sgs 外层 | d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6 |
| sgsGame_a.sgs 外层 | 96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31 |
| 解包客户端 sgsGame_a.sgs 成员 | fe15f698c367fa5351f6ab8c3a4664de3bcb48f3d6dd4f6750eb4a300dffd6e6 |
| 解码 hd_roguelike.sgs | 97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72 |
| 解码 cha_spell.sgs | e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219 |
| 解码 sys_playcard.sgs | 05584b30f7986c06158ee6e47f78417b3d5f1c7fbc61174cfad26e12a5ee64ab |

`fightID=2497`、`Ggroup=2497`，标题为狭路相逢。完整阵容只有 `General.a=2497` 一行：`b=4861`、`c=江东老兵`、`d=1000283`、`h=100`。4861是士兵身体，使用统一角色 `character:shanhe-source-soldier-4861`；本地 `supporter` 是缺图回退，并非已核官方士兵肖像，也不借用凌统、朱桓身体。

原始体力/上限4/4，通常摸牌2，随机初手4，护甲0、护甲回复0、护甲上限5、额外出杀0、额外伤害0，无隐藏成员或额外 `att_*` 属性。`NewbieGroup=22497` 是另一配置，不追加为本组随从。普通池14、16、18、19、20、21、22、26引用本组，全部开放后覆盖56个第二章普通城市；最终战斗数值由共用来源成长按真实难度、天数、章节及可见人数1投影。

| 源难度 | 累计技能字段 | 累计固定装备 |
| --- | --- | --- |
| 1—10 | `v=409` 旋风 | `t=20511` 青釭剑，梅花6 |
| 11—15 | 以上加 `getspell_KN=396` 奋励 | 以上加 `equip_KN=20571` 白银狮子，方块A |
| 16—20 | 以上加 `getspell_EM=397` 平寇 | 保留以上两张 |
| 21—25 | 保留全部三个技能 | 以上加 `equip_LY=20771` 照骨镜，红桃A |

装备原始花色/点数 `g/f` 分别为3/6、4/1、2/1，按用户既定本地花色编码约定映射为表中牌面。固定装备额外加入随机初手，不在开局直接装入装备区；这一放置位置沿用用户已确认的本地规则。21档增加装备不增加新的武将形态；`GetGeneralId(21..25)` 继续选择含三个技能的形态，装备由独立来源配置累加。照骨镜采用独立 `CardKind.ZhaoguMirror`、技能 `shanhe:zhaogu-mirror-6018` 与 `StandardShanheZhaoguMirrorPackage`。

`t` 的字段含义本批已直接核实，不再只凭装备内容推断：解码 `hd_roguelike.sgs` 的 `root.abbreviation` 明确包含 `{"Long":"equip","Short":"t"}`。固定客户端成员中：

- `aA.initAbbreviation`（字符偏移1195879）建立 `shortToLongObj[Short]=Long`。
- `nQi.GeneralDict`（17694588）读取 `Root.General`，创建 `iz` 并调用 `jmt.copyObjL2S(s,t,this.shortToLongObj)`。
- `jmt.copyObjL2S`（4579195）逐键执行 `r=s[h]||h,i[r]=t[h]`，因此原始 `t=20511` 成为 `iz.equip="20511"`。
- `iz.EquipCards`（3187022）把 `this.equip.split(";").map(Number)` 作为基础装备，再通过 `equip_PT/ZD/KN/EM/LY` 的增删表与当前章节表合并。这直接证明基础青釭剑及后续白银狮子、照骨镜属于累加配方。

409（`XuanFeng`）在弃牌阶段弃过至少两张手牌，或失去装备区牌后，可以弃至多两名其他角色共至多两张牌，复用 `classic:xuanfeng`；不采用能额外移动装备或放宽失牌窗口的其他版本。

396（`FenLi`，源名称为奋励）在手牌最多时可跳摸牌、体力最多时可跳出牌、装备区有牌且装备最多时可跳弃牌。复用精确既有 `wushuang:skill-3125`：原生 `SkipWushuangFenliPhase` 要求这个身份、本人真实阶段父窗口及相应最多条件，没有单骑无双模式门禁。因此包通过 `EmbeddedSkillProgramCatalog.Definition("wushuang-skills-a", FenliSkillId)` 导入同一完整定义，沿用既有山河图单定义导入方式，不复制操作改名。原完整注册者为 `StandardWushuangContentPackage`（`standard-wushuang`）；无需为本士兵引入其全部模式与商品。此本地3125是既有无双目录标识，不是官方本次 `cha_spell` 的3125来源身份。

397（`PingKou`）在自己的真实回合结束后，按本回合实际跳过阶段数，可以对至多等量其他角色各造成1点伤害，复用 `mobile:pingkou`，与奋励实际跳阶段流程相接。包只依赖已包含旋风、平寇的 `standard-classic-generals`，本批不新增技能规则、Core机制或逐角色检查。
