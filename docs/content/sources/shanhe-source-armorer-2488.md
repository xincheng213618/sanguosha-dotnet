# 锻甲型将领周昕2488

来源为[官方配置](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)的既有同版冻结快照，没有重新下载当前服务器版本。Config 外包 SHA-256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`，Client 外包为 `96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31`；本次重新解码的 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，`cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`，`character.sgs` 为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`。

普通 `Fight.fightID=2488` 使用 `Ggroup=2488`；`NewbieGroup=22488` 是另一新手配方。原普通组只有一员，来源显示名为周昕，不把内容类名 Armorer 当作来源姓名：

```json
{"a":2488,"startChapter":1,"b":4871,"c":"周昕","d":"1000204","f":0,"h":100,"k":5,"l":5,"m":0,"n":0,"o":5,"p":2,"q":4,"r":0,"otherad":0,"v":"14095","getspell_KN":"3414","getspell_EM":"14098","pvpspell":"3189"}
```

原身体为将领4871，体力5/5、基础摸牌2、随机起手4；没有固定牌、额外属性、隐藏成员、关底标记或二阶段。护甲、护甲回复、额外出杀及额外伤害基础值均为0，护甲上限字段5。人物表4871为通用将领，保留 `character:shanhe-source-general-4871` 身份；原头像1000204已按下述来源链核实。运行时保留 `supporter` 回退键，桌面按本族精确运行时ID绑定原卡，不借用凌操或韩当身体。`pvpspell=3189` 不加入山河图普通技能。

| 来源难度 | 累计技能 |
| --- | --- |
| 1—10 | 14095锻甲 |
| 11—15 | 锻甲、3414独进 |
| 16—25 | 锻甲、独进、14098辎重 |

21—25没有额外新技能，保留16档全部能力；原始5/5身体不随本内容包的技能档改变。

## 完整能力与本地随机池

**14095锻甲**原文：“准备阶段，若你没有防具，则将一件随机防具置入你的装备区。”定义为 `shanhe:source-duanjia-14095`，本人准备阶段强制检查实际防具与可用防具栏，通过 `placeRandomMissingArmorDeckEquipment` 将随机防具的真实实体直接置入装备区。它限定防具，不误用辎重的任意空装备栏；不是摸到手牌后再出牌，也不会覆盖已有防具。来源没有写“锁定技”，本包保留强制触发而不自行添加锁定标签。源行的另一提示 `ac` 涉及铁甲胄付款，并未写在主规则 `o` 内，不据提示文本给锻甲附加其他防具效果。

**3414独进**原文与 `ol:dujin` 全文相同：“摸牌阶段，你可以多摸X+1张牌（X为装备区里的牌数的一半且向上取整）。”沿用真实本人摸牌阶段的可选额外摸牌及 `equipmentHalfCeilingPlusOne` 数量表达式；不添加凌操其他能力。

**14098辎重**原文与 `shanhe:elite-zizhong` 相同：“准备阶段，若你有空的装备栏，则将一件随机装备置入你的装备区。”精确复用精英韩当内容包中的 `placeRandomEmptySlotDeckEquipment`，按真实仍空且可用的装备栏筛选后直接入场。包声明 `shanhe-elite-handang` 和标准技能包依赖，由工厂统一加载一次；不重复注册辎重，也不获得韩当身体及其他技能。

本次实际山河图流程检查发现，用户已要求山河图牌堆排除装备，旧辎重从实际牌堆找装备会一直没有候选。保留该牌堆约定，山河图的本地随机池改取本场注册牌堆配方中、过滤前的印刷装备模板；筛选仍有容量的栏位后，按配方中的模板条目等概率选择，并创建一个有独立ID的真实实体，从游戏外直接移入装备区，记录创建与装备入场事件。它不进入摸牌堆，也不经过用牌宣告或替换已有装备；离装、夺取和后续使用仍走正常牌区流程，洗牌时继续排除装备。锻甲还要求当前完全没有防具，即使扩展了第二个防具栏也不会在已有防具时补第二件；辎重仍可填扩展栏位的剩余容量。非山河图模式保持原来的实际牌堆取牌流程。无合法模板或栏位时不生成牌。冻结主文和Client没有公开服务器候选全集或权重，这个模板来源是明确的本地执行约定，不宣称已还原官方全部随机装备分布。

## 实际投放

普通池23、24、25、27分别以1855、1280、1280、1280为前提，覆盖第三章洛阳5101—5107、河内5111—5117、酸枣5121—5127、崤函道5141—5147，共28座普通城市；来源难度1、11、16起分别开放7、21、28城。`startChapter=1` 不替代真实池、区域与难度开放条件。战斗奖励字段为 `reward=125`、`reward1=401`；本地入口、成长、奖励由地图层编排，不凭编号推定服务器抽取权重或未公开效果。

## 2026-10-29 原生席位图与预览身份

完整链为 `Fight2488.Ggroup=2488 → General raw0,b=4871,c=周昕,d=1000204 → root.pveskin.skinID=1000204`。皮肤行明确指定 `seatpic=generalface_1000204`、`headpic=1000204`、`diagrampic=100020400`。人物表4871是“将领”、类 `ShiBing71`，没有 `au/av` 头像重定向；本行的PVE皮肤覆盖载体默认图，不按同名、共享载体或相似面孔借图。

冻结人物表解码SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`，皮肤表解码SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`。当前另行核实的公网Config/client包装SHA已变化，但Character解码字节仍与该冻结值完全一致；旧包装与当前包装证据分别记录于[山河NPC美术来源](../shanhe-npc-art-sources.json)，本批不重抓配置或用当前包装SHA覆盖冻结General/Fight/皮肤资料。

同版客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 经 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 得到[官方原PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000204.png)。已取得HTTP200、`image/png`，并目视核实原生卡面。原图146×172 RGBA、62207字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000204.png`；文件SHA-256为 `9732f54ece954c75481f19356a7cfeaed4d971aadb04aed3b263d9812381f4d3`，RGBA像素SHA-256为 `4a83e964882a925e736061de5e2fa9bd3485e6dfa6f4311c541f27c46a224aec`。未裁剪、缩放、换色或重编码。

此组只有上述一名源成员，`startChapter=1,f=0`；原行没有 `g/hide/generalShow`，按源默认false/false/0，无主帅、隐藏成员或后续章节成员。完整单人预览保留大卡，不从首席推断主帅。下列运行时ID与上轮全25档实际发布配方盘点一致；美术绑定不改变PortraitKey、VariantId、内容指纹、印刷身体、技能、装备或成长。

| 来源难度 | 精确运行时ID |
| --- | --- |
| 1–10 | `shanhe:source-armorer-2488-base` |
| 11–15 | `shanhe:source-armorer-2488-hard` |
| 16–25 | `shanhe:source-armorer-2488-nightmare` |
