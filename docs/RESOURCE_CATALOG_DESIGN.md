# 武将与卡牌资源目录

规则内容与静态素材分别维护：`ContentRegistry` 决定武将、技能、卡牌和模式的规则身份；资源目录保存图片、录音、来源与适用绑定。素材存在不表示对应武将或规则已经实现。

| 关系 | 当前表示 |
| --- | --- |
| 武将与技能 | `ContentGeneralDefinition.Id` → `SkillIds` → `ContentSkillDefinition.Id` |
| 武将与立绘 | `general-art-catalog.json` 按外观键保存默认皮肤和 skins |
| 卡牌与牌面 | `card-art-catalog.json` 关联牌种与本地图片，运行时叠加实体花色点数 |
| 短音效 | `GameSound` 与 `Assets/Audio/manifest.json` 对应本地合成 WAV |
| 音乐与录音 | `game-audio-catalog.json` 的 assets 保存文件，bindings 保存适用关系 |
| 图鉴装饰 | `gallery-ui-assets.json` 记录背景、卡框、徽章与 atlas 帧 |

## 运行关系

```text
内容武将 ID → 技能 ID → 技能定义／规则程序
        ↓ 玩家可见身份
武将外观键 → 本机选中的 skinId
        ↓
已提交技能／阵亡事件 → 音频 bindings → asset → 本地播放器
```

图鉴列出该武将已核实的台词，支持逐句试听。对局自动配音要求武将版本、当前皮肤与技能／阵亡用途匹配；尚未匹配本地皮肤的录音可以列出试听，不凭相似姓名自动套用。

`sourceGeneralIds`、`sourceSkinIds`、`sourceSkillIds` 与 `sourceGeneralClasses` 保留来源系统标识；本地 `generalKey` 和 `skinId` 用于素材关联。普通、界、神、SP、谋等版本独立对应，不能仅凭中文同名合并。

文件来源记录包括原始地址、来源证据、格式、字节数、时长和 SHA-256。OGG 导入为 PCM WAV 时分别记录原始与交付哈希，台词文字和绑定关系独立于文件保存。同一录音可供多个绑定引用，无需重复存储。

## 导入与表现边界

### 代码、目录与素材的交付

| 内容 | 交付方式 | 读取入口 |
| --- | --- | --- |
| XAML 和界面代码 | 编译到 WPF 程序集 | WPF 界面与主题加载 |
| 素材关联、来源和规则资料 JSON | 按现有逻辑名嵌入程序集 | 对应目录加载器 |
| 图片、音乐、配音和音效 | `Content` 文件，构建和发布时按需复制到程序旁的 `Assets/` | `AssetFiles`，图片解码使用 `AssetImages` |

大型素材不作为 WPF `Resource` 或 `EmbeddedResource` 编入主程序集。新增武将和皮肤继续沿用目录与文件，不为图片增加新的程序集。保持素材文件和来源哈希不变；外置会缩小主 DLL、分开代码和素材的更新成本，完整离线游戏仍需要相应素材文件。

[`AssetFiles`](../src/CardGame.Wpf/Resources/AssetFiles.cs) 以 `AppContext.BaseDirectory` 为唯一安装根目录，接受目录中既有的 `src/CardGame.Wpf/Assets/...` 标识和运行时的 `Assets/...` 路径。前缀仅用于兼容目录标识，运行时不访问源码。绝对路径、网络地址和目录穿越会被拒绝，音频目录继续约束自己的子目录。

[`AssetImages`](../src/CardGame.Wpf/Resources/AssetImages.cs) 统一处理立绘、皮肤、卡面和活动图片，按路径及解码宽度共享冻结后的弱引用缓存。加载后释放文件流；缺失或损坏的可选图片返回空图，不缓存失败。游戏中的文字、卡牌身份与交互继续可用。固定 XAML 装饰通过 WPF `pack://siteoforigin:,,,/Assets/...` 读取同一安装目录，主题 XAML 仍作为编译资源交付。

离线发布仍由既有打包流程复制和校验完整目录，校验失败必须报错。日常定向检查覆盖素材没有重新嵌入主程序集、目录素材随项目引用复制、工作目录独立性、图片解码与缺图恢复；界面布局沿用现有 WPF 检查。

`tools/Import-GameAudio.ps1` 导入音频，立绘与界面素材工具见 [`GENERAL_GALLERY.md`](GENERAL_GALLERY.md)。JSON 目录随程序离线交付，运行时不需要外部资源数据库。新增素材通过目录和绑定表达，不为每个武将增加播放器特判。

`GeneralVoiceProjector` 只消费已提交事件与公开快照，资源配置不触发规则，也不泄漏暗置身份。技能程序和运行模块的关系见 [`CONTENT_MODULE_ARCHITECTURE.md`](CONTENT_MODULE_ARCHITECTURE.md)。

皮肤、音乐、配音、总声音和动画属于 `PlayerPreferences`。后台、静音和关闭会取消播放，回前台恢复当前场景音乐，不追播旧技能，详见 [`BATTLE_AUDIO.md`](BATTLE_AUDIO.md)。
