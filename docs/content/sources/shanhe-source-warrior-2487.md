# 山河图武卫兵：来源2487、技能290/488/723

原始资料来自官方OL同一份 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)。冻结外包 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，`cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

`Fight.fightID=2487` 指向 `Ggroup=2487`。该组唯一成员 `General.a=2487,b=4861,c=武卫兵,k=4,l=4,o=5,p=2,q=4`，固有技能 `v=290`，困难累加 `getspell_KN=488`，炼狱累加 `getspell_LY=723`。本包保留士兵4861的4/4基础身体；起手和难度成长由征程的来源数据投影。统一 CharacterId 为 `character:shanhe-source-soldier-4861`。运行时保留 `supporter` 回退键，桌面可按下列精确来源卡面及运行时ID绑定显示。

| 原难度 | 本地形态 | 累积技能 |
| --- | --- | --- |
| 1–10 | `shanhe:source-warrior-2487-base` | 推锋290，复用 `ol:tuifeng` |
| 11–20 | `shanhe:source-warrior-2487-hard` | 推锋、诱敌488 |
| 21–25 | `shanhe:source-warrior-2487-hell` | 推锋、诱敌、裸衣723 |

## 原生卡面

已于2026-10-29核对完整普通组链 `Fight2487.Ggroup=2487 → General raw0,b=4861,d=1000230 → root.pveskin.skinID=1000230`；角色4861为“士兵”、类 `ShiBing61`，无 `au/av` 默认头像重定向。明确的PVE皮肤覆盖默认载体卡面，不按武卫兵名称或推锋等技能借其他具名武将图。皮肤行指定 `seatpic=generalface_1000230`、`headpic=1000230`、`diagrampic=100023000`；解码 `cha_gs_dbs_fs_skininfo.sgs` SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`，解码角色表SHA-256为 `92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`。

客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 由 `N2t.SeatImgUrl=general/seat/static/` 和桌面 `res/runtime/pc/` 生成[官方PNG](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000230.png)。HTTP200、`image/png`，已目视铁盔披甲男性武卫原卡。原生146×172 RGBA、48811字节，原字节导入 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000230.png`；文件SHA-256为 `ba500d75405f9cce901354267714559b810eea29b85b6eb8712039ab924e68e8`，RGBA像素SHA-256为 `6faaf4b669f641da00f1a0075a1b2c888ed21f02c3e20172cf376483f613efe4`。未裁剪、缩放、换色或重编码；仅绑定表中三个精确运行时ID。成员 `startChapter=1,f=0`，缺省 `g/hide/generalShow` 按源默认false/false/0；`NewbieGroup=22487` 不作隐藏成员，不能凭首席增加主帅标记。

诱敌488为结束阶段令其他角色弃置自己一张牌；该牌不是杀时，取得对方一张牌。它复用现有选牌、真实移动和种类判断，使用独立来源技能ID，不带 `classicIdentityMode` 条件。装备区仍是可选区域，杀的判断包含本地完整杀族。

裸衣723是 `LuoYiPo`，不是14191 `LuoYiPoRogue`。它有两次选择：先决定是否亮出固定三张牌，看到牌后再决定是否跳过摸牌。接受第二选择时取得其中全部基本牌、武器和决斗，弃置其余亮出的牌；拒绝时全部亮出牌进入弃牌堆并正常摸牌。接受后获得直到下次自己的回合开始的杀/决斗来源伤害+1。它不按当前体力增加亮牌数或伤害。

[当前官方OL界许褚页面](https://www.sanguosha.com/hero/315) 与缓存723主文一致，并说明可以看牌后决定。弃牌去向采用[官方OL《武将技能描述修改通用原则》](https://www.sanguosha.com/news/20160808_3932_1715)第4条（页面日期2016-08-17）：牌堆顶亮出牌未另指去向时默认进入弃牌堆。该去向是明确官方通用规则的应用，不声称已捕获此冻结版本的服务器最终移动消息；十周年内测FAQ不作为本来源版本的替代依据。

同一客户端外包 SHA256 为 `96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31`；其中 `sgsGame_a.sgs` 为 `fe15f698c367fa5351f6ab8c3a4664de3bcb48f3d6dd4f6750eb4a300dffd6e6`。注册 `LuoYiPo -> sMi` 的第二选择同时启用“是/否”；没有合资格牌时标题明确为“是否获得裸衣效果(无法获得牌)”。因此本来源不增加“至少取得一张牌才可跳摸”的门槛。客户端只发送选择并消费服务器移动通知，未在该类自行计算最终去向。

实现复用公开亮牌、绑定牌集过滤、原生选择、条件跳过当前摸牌阶段、等待真实移动完成及来源伤害修正。所有牌实体仍属于原ProgramSkillFrame；第二次接受才提交当前Draw父流程的真实跳过记录。现有 `boundary:luoyi` 提前承诺跳摸，`boundary:luoyi-mobile` 强制亮牌且要求非空收益，均未被改写或误当作来源723。

前向检查覆盖首次可选发动、亮牌后拒绝的三实体弃置与正常摸牌、混合牌接受的原实体获得/清理、无合资格牌仍接受，以及实际杀加伤在下一次自己回合开始到期。检查不完成整场战斗。
