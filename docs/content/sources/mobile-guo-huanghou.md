# 郭皇后（普通移动版）

核查日期：2026-10-26。普通身份 `mobile:guo-huanghou`，人物身份沿 `character:guo-huanghou`；魏、女、3体力；乱世英杰2016，图鉴 `fame-6`。矫诏注册81700，武将及将池81799。使用[官网经典38000.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/38000.jpg)，建议立绘键 `mobile-guo-huanghou`。

## 当前版本来源

- [官网人物217](https://www.sanguosha.cn/pc/hero-detail-217.html)是明元郭皇后的普通身份，和郭女王及界郭皇后不同。一级、二级写明展示真实手牌、最近其他角色声明和固定此牌。三级同一段先写展示并自己声明，随后却写任意一张手牌当任意牌，内部不一致，不能据此省去前面的实体和声明限制。
- [当前移动矫诏汇总页](https://wiki.biligame.com/msgs/矫诏)与[殚心汇总页](https://wiki.biligame.com/msgs/殚心)均显示2026-10-22更新。普通郭皇后底级保持展示一张手牌、最近其他角色声明基本牌；殚心依次只替换可声明类别与声明者：先扩为基本牌或普通锦囊，再将最近其他角色改为自己。没有删除展示、固定实体、固定牌名或不能指定自己的文本。
- 对应[普通矫诏专页40792](https://wiki.biligame.com/msgs/index.php?title=矫诏/郭皇后&oldid=40792)、[殚心专页40789](https://wiki.biligame.com/msgs/index.php?title=殚心/郭皇后&oldid=40789)及[人物页33725](https://wiki.biligame.com/msgs/index.php?title=郭皇后&oldid=33725)可作为版本定位。本次实时核查使用可读取的当前技能汇总页交叉确认普通/界版分栏，没有使用界版正文替代普通身份。

## 采用规则及复用

矫诏三级始终是出牌阶段限一次，展示一张自己手牌、固定一项声明名称，本回合仅能将该实体当该声明牌使用，且不能以自己为目标。底级由距离最近的其他角色声明基本牌；第一次殚心升级后可声明基本牌或普通锦囊；第二次升级改为自己声明，其余限制保留。基本牌与仅响应牌的使用方向沿既有经典底级定义，不扩大为任意打出或额外允许用途。

殚心的真实受伤后摸一或至多升级两次与 `classic:danxin` 完全等价，直接引用该技能及 `declared-card-tier` 状态。升级继续走 `GameEngine.ProgramSkillChanges.cs` 的原帧突变入口，没有复制升级或布尔状态实现。

经典矫诏的顶级是无须展示或声明的直接转换，和当前移动普通版有实质差异。因此新增 `mobile:jiaozhao`，所有声明转换均为 `declaredEntity:true`，保留原生可用牌名和使用方向，覆盖至tier2，不能拿另一张同名手牌替换展示的实体。

## 执行与兼容契约

`declareBoundCardNameUntilTurnEnd` 增加可选 `selfDeclarationFromTier:2`。仅显式值2合法；省略保持经典原行为。新策略在最高级直接将原 `ProgramCardDeclarationDraft` 的声明者设为拥有者，冻结的候选席位只含拥有者；仍使用原公开绑定、原手牌实体和既有牌名选择。原invariant仅在该策略及冻结tier匹配时允许自己声明，其他策略继续要求最近其他角色。

没有更改声明DTO或宿主接口，没有新opcode、规则epoch、schema或内容包版本。选择恢复仍在原技能帧上；声明完成退休临时draft，使用付款、不能指定自己、本回合有效、当前原实体必须仍位于自己的手牌区等条件沿已有配置转换机制检查。经典的顶级直接转换和原低级最近其他角色声明均保留。

## focused与共享回归

`MobileGuoHuanghouChecks.TwiceNativeUpgradedSelfDeclarationKeepsFixedEntityAndNameCold` 通过两次真实原生伤害及殚心选择升级，确认最高级在未声明前不能转换任意手牌，仍向所有观众展示唯一实体并由自己声明。一次声明窗Cold保留原帧及固定实体；同名替代牌提交真实拒绝且不支付；原实体按固定声明的决斗名称完成原生使用，实体付款及结算只发生一次，阶段次数不能重复使用。

底层沿已有 `Fame2016ConversionChecks.DeclarationEntityPrivacyAndReplay`、程序选择draft冻结和共享Skill changes检查回归，不复制经典低级声明隐私或响应矩阵。验证由集成任务串行执行，定向结果不表示全量通过。
