# 卡牌与牌桌素材

[card-art-catalog.json](card-art-catalog.json) 维护卡牌和组件的来源、文件、裁切、适配与 SHA-256。牌面位于 `src/CardGame.Wpf/Assets/Cards/`，按 `CardKind` 绑定，当前覆盖范围由清单和 `CardCatalog.ImplementedCards` 核对。

## 素材来源

| 类型 | 处理方式 |
| --- | --- |
| 官方固定图集 | 校验 `cards.webp` 与 `cards.atlas` 输入哈希，按精灵帧裁切牌面、点数、花色与装备条 |
| 官方完整牌图 | 使用清单的 `officialCardFace` 来源及缩放参数，保留原印刷内容 |
| 官方插画 | 使用 `officialIllustration` 来源，保持宽高比缩放并在透明画布补边 |
| 公开图鉴完整牌图 | 使用 `publicCatalogueCardFace` 来源，核对原图哈希与 186×260 尺寸，逐字节复制；不标为已确认官方素材 |
| 项目生成素材 | `projectGenerated` 记录现有生成脚本、尺寸与哈希，导入器验证并保留文件 |

鬼龙斩月刀对应固定图集中的独立牌面与装备条。赤血青锋使用来源已记录的 500×500 官方插画，等比例缩为 186×186，置于 186×260 透明画布；刑天破军斧使用 93×130 官方完整牌图，以最近邻放大两倍。`GeneralWeapon` 和 `RedBloodBlade` 使用项目生成素材，原始脚本为 `tools/Build-GeneratedWeaponArtwork.ps1`。

山河图 S5 的 `ShanheXingtianAxe` 与原 `XingtianAxe` 保留独立规则，共用已有 `XingtianAxe.png` 牌面和 `equip-XingtianAxe.png` 装备条。其显示不再回退为通用剑，也不再在已经印有名称的刑天牌图上叠加另一层运行时名称。

山河图三首使用[公开图鉴商品 4061](https://s.xiaochao.org/data/cards.json)所列的[原完整牌图](https://s.xiaochao.org/data/images/b82d9ccf44e19a7b160c1a00.png)，原样保存为 `ShanheSanshou.png`。图内已有“三首”、盔甲图标与装备标签，没有花色点数；运行时只补当前牌的花色点数，不重复叠名称。该缓存图的官方上游帧尚未确认，清单保留公开图鉴来源。尚无可确认的原始装备横条，装备栏继续使用现有文字回退。

黑光铠使用[同一公开图鉴商品 4050／4216](https://s.xiaochao.org/data/cards.json)共用的[原完整牌图](https://s.xiaochao.org/data/images/f5ee9d6839716f0f59144628.png)，钩镶使用[商品 4153 的原完整牌图](https://s.xiaochao.org/data/images/828b194f65658419932ce796.png)，逐字节保存为 `BlackLightArmor.png`、`GouXiang.png`。两图均为186×260，保留原名称、插画与装备标签，没有花色点数；不重复叠名称，花色点数绑定运行时状态。完整SHA-256分别为 `F5EE9D6839716F0F59144628FF94ED2AA05D250CD205E80D35E0D061D202D414`、`828B194F65658419932CE796C700793CCDE2F7A3B998E79F4552C989124CC032`。图鉴同名描述与本地规则相符，但商品中的实体编号不等同本地来源编号（例如钩镶图鉴20833与来源20883）；官方上游帧尚未确认。两件装备均无已核实原横条，装备栏保留文字回退。

无双方天戟使用[同一公开图鉴的商品 4026／4197](https://s.xiaochao.org/data/cards.json)共用的[原完整牌图](https://s.xiaochao.org/data/images/0ecbb32fc66c2c3be8c682a2.png)，替换 `WushuangHalberd.png` 的项目绘制牌面。同名图鉴描述与本地的“杀造成伤害后，摸一张牌或弃置目标一张牌”一致，图上攻击范围 4 也与本地装备定义一致。牌图原样保留名称、武器、范围和装备标签，花色点数仍取运行时状态。装备横条保留现有项目素材，未将其标为公开图鉴或官方原条。

牌面不烘焙运行时花色、点数或转化效果。手牌、起手牌、公开牌与指南共用相应模板；花色点数来自当前可见状态，未知或缺失素材显示文字卡牌。图片不进入规则存档或 Core 内容定义。

## 重建与校验

在仓库根目录运行 PowerShell，ffmpeg 需在 PATH 中：

```powershell
pwsh -File .\tools\Import-CardArtwork.ps1
pwsh -File .\tools\Import-CardArtwork.ps1 -ValidateOnly
pwsh -File .\tools\Import-CardArtwork.ps1 -Kinds ScarletBloodSword,XingtianAxe
```

`-SourceDirectory` 指定已有输入目录，`-Ffmpeg` 指定解码程序。导入器校验来源域名、输入哈希、尺寸、atlas 帧、缩放比例与输出尺寸，所有选中条目验证后才写交付图片与清单。`-ValidateOnly` 不改交付图片或清单；`-Kinds` 仅处理指定牌面，不改其他图和组件。

项目生成条目要求现有文件和记录哈希一致，缺图或冲突会明确报错。官方插画使用 Lanczos 等比例缩放与透明补边，完整牌图按记录的最近邻方式放大；输出哈希写回清单。

公开图鉴完整牌图仅接受 HTTPS `s.xiaochao.org/data/images/<SHA-256 前 24 位>.png`，校验完整 SHA-256 与原尺寸后直接复制；该路径不调用图像转换程序。

## 牌桌与技能

[table-art-catalog.json](table-art-catalog.json) 保存「天下牌局」背景、边框、装备空槽、操作与技能按钮的来源、atlas 帧和哈希。装备区从玩家快照显示真实牌面、花色和点数，中央公开牌使用已公开事件资料。

出牌操作区提供确定、取消、结束；没有对应操作时按钮禁用。本地对局没有操作时限。技能按钮按内容技能 ID 和当前技能拥有者重新查找合法动作，只有可执行动作才能进入选择；费用和目标选齐后由主按钮或 Enter 提交。当前不能主动使用的技能保留状态与说明。

装备效果、共享入口与额外激活按合法动作进入技能区或补充操作区；技能选择期间避免重复提交草稿。具体操作见 [主动技能](../SKILL_INTERACTION.md)、[响应](../HAND_RESPONSES.md) 与 [战斗反馈](../BATTLE_FEEDBACK.md)。

构建／包检查依据清单核对牌面、组件与牌桌资源的哈希和尺寸。原素材中的印刷文字属于图像，工具提示与指南正文使用当前内容规则。
