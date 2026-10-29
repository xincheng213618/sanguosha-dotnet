# 山河图《空谈无益》：来源2480荀悦

原始资料来自官方OL的 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)。冻结外包 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`，解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。技能描述对应同缓存 `cha_spell.sgs`，SHA256 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

`Fight.fightID=2480,Ggroup=2480,text=248000` 为《空谈无益》。它只有一名成员：`General.a=2480,b=4871,c=荀悦,d=1000195,h=100`。`character.a=4871` 是“将领”身体，不是另一个同名武将模板。基础字段为 `k=3,l=3,p=2,q=4`，即3/3、摸牌2、初始牌4；`m=0,n=0,o=5,r=0,otherad=0`。原组没有固定装备、隐藏成员或本组 `att_*` 增长。征程仍使用来源难度、日期、章节和实际人数的通用成长，因此基础身体不等于最终实战数值。

本包统一使用 `character:shanhe-source-general-4871`，印刷3/3。原外观串 `1000195` 已按下述来源链核实为官方原卡；运行时保留 `supporter` 回退键，桌面按本族精确运行时ID绑定，不借同名将的印刷体力或肖像。

| 原来源难度 | GeneralId | 累积技能 |
| --- | --- | --- |
| 1–10 | `shanhe:source-xunyue-2480-base` | 强识484 |
| 11–20 | `shanhe:source-xunyue-2480-hard` | 强识、明哲3205 |
| 21–25 | `shanhe:source-xunyue-2480-hell` | 强识、明哲、誉虚889 |

强识484 `QiangZhi` 的原文限定在本次出牌阶段：开始时可展示其他角色一张手牌，之后该阶段使用同类型牌可摸一张。它采用独立ID `shanhe:source-qiangzhi-484`，复用 classic 强识的单牌展示、类别记录和真实用牌后可选摸牌链；在 `cardUseCommitted` 的自身用牌条件中加入 `cardActionPhaseIsPlay`。三个类别状态限定 `playPhase`，自己的每次出牌阶段开始重置，自己的出牌阶段结束清理，不使用 classic 版本的“本回合”持续范围。

选牌细节沿用既有 `revealTargetHandCard` 的 `mode=random`，即从对方未公开手牌中盲选一个真实实体再公开。原技能没有授予观看全手牌的能力；缓存客户端 `QiangZhi -> hHi` 含选牌界面，但未据此证明服务器允许查看整手牌。因此本地复用既有盲选操作，不附加界强识的全手观看，也不声称已复刻未核的服务器选牌界面。

明哲3205 `MingZhe2` 复用 `ol:mingzhe`：锁定，在自己出牌阶段外真实失去红色牌后展示原牌并摸一张。它不是移动版的回合外可选使用/打出/弃置版本。誉虚889 `YuXu` 复用 `ol:yuxu`：自己的真实出牌阶段用牌后可摸一张，接受后该阶段下一次用牌结算后强制弃一张且不能再次以此法摸牌。两个来源只授予对应技能，不捎带原武将包的其他印刷技能。

同阶段的响应式“使用”也属于原文用牌范围，例如在自己的出牌阶段使用无懈可击。独立强识的 `cardUseCommitted` 显式启用 `includeResponseUses`，复用原生已付响应和其原父流程；既有原生资格负责排除单纯“打出”，不把所有响应扩成使用。

独立前向检查使用真实来源基础身体和实际敌方手牌，验证只公开一张原实体、同阶段不同类型用牌无摸牌、同类型原生用牌的可选摸一张，以及实际出牌阶段结束清除类别。下一次真实己方出牌阶段拒绝展示后，同类型用牌不继承前一阶段效果。同一检查另含一个短的锦囊类流程：支付实体无懈后可选摸一张，随后返回原无懈窗口及父决斗，两个实体都只支付、结束一次。它只推进到必要的阶段边界，不完成整场战斗。

## 2026-10-29 原生席位图与预览身份

完整链为 `Fight2480.Ggroup=2480 → General raw0,b=4871,c=荀悦,d=1000195 → root.pveskin.skinID=1000195`。皮肤行明确指定 `seatpic=generalface_1000195`、`headpic=1000195`、`diagrampic=100019500`。人物表4871是“将领”、类 `ShiBing71`，没有 `au/av` 头像重定向；本行的PVE皮肤覆盖载体默认图，不按同名、共享载体或相似面孔借图。

冻结人物表解码SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`，皮肤表解码SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`。当前另行核实的公网Config/client包装SHA已变化，但Character解码字节仍与该冻结值完全一致；旧包装与当前包装证据分别记录于[山河NPC美术来源](../shanhe-npc-art-sources.json)，本批不重抓配置或用当前包装SHA覆盖冻结General/Fight/皮肤资料。

同版客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 经 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 得到[官方原PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000195.png)。已取得HTTP200、`image/png`，并目视核实原生卡面。原图146×172 RGBA、49111字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000195.png`；文件SHA-256为 `436ce41ea05dd01364c2b85c9537569ff323aec98976a90a685835956c0938ba`，RGBA像素SHA-256为 `4b8810f780e1b3e69528c7cdd3eee311b3b691ff7ab9cc7e28153eacd116b64d`。未裁剪、缩放、换色或重编码。

此组只有上述一名源成员，`startChapter=1,f=0`；原行没有 `g/hide/generalShow`，按源默认false/false/0，无主帅、隐藏成员或后续章节成员。完整单人预览保留大卡，不从首席推断主帅。下列运行时ID与上轮全25档实际发布配方盘点一致；美术绑定不改变PortraitKey、VariantId、内容指纹、印刷身体、技能、装备或成长。

| 来源难度 | 精确运行时ID |
| --- | --- |
| 1–10 | `shanhe:source-xunyue-2480-base` |
| 11–20 | `shanhe:source-xunyue-2480-hard` |
| 21–25 | `shanhe:source-xunyue-2480-hell` |
