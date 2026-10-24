# OL 界关平（2026）

## 一手来源与版本身份

- [2026-06-05 官方首发公告](https://www.sanguosha.com/news/20260529_8264_4513)：明确界关平首发，可通过酒馆招募取得。
- [2026年7月排位将池官方公告](https://www.sanguosha.com/news/20260630_5762_0815)：将界关平列入 OL 界限突破将池。
- 2026-10-25 读取[官方公开客户端 Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)，按[公开客户端自身解码代码](https://web.sanguosha.com/220/h5_2/sgsGame_a.sgs)的 AES-CFB128、gzip 流程核对 `character.sgs` 与 `cha_spell.sgs`。复用仓库已有工具中记录的解码合同，没有运行整表写入。
- `character.sgs` 的角色766：`GuanPingPo`、标识“界”、蜀、男、4体力，技能映射 `j=446`、`k=4011`。`cha_spell.sgs` 中446为 `LongYin`（龙吟），4011为 `JieYong`（竭勇）。[同版 OL 百科界关平](https://wiki.biligame.com/sgsol/?curid=10822)与两条正文一致。
- 当前[官网武将列表](https://www.sanguosha.com/hero)未收录界关平，`/hero/766`实际跳转至404；[177详情](https://www.sanguosha.com/hero/177)仅为经典关平，不能冒充界版详情。

配置 SHA256：`d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；客户端包 SHA256：`96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31`。解码的 `character.sgs` SHA256：`92f00132ed944b279066f46655478bde6a2f3b9f1c006d086c197fbbbada49de`；`cha_spell.sgs` SHA256：`e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。配置中存在角色不单独证明已上线，此处由两篇官方公告交叉证明。

## 完整规则

- 龙吟：当一名角色于其出牌阶段内使用杀时，你可以弃置一张牌，令此杀不计入该阶段使用次数；若此杀为红色，你摸一张牌。
- 竭勇：你可将手牌中最后一张红色牌当杀使用或打出。

OL 本次保留原生龙吟，并增加竭勇。不能替换为十周年的“同点数复原竭忠”版，不能替换为谋关平武威；角色自身也没有限定补牌技“竭忠”。“最后一张红色牌”按当前手牌中的红色数量为1判断，并非最后一张手牌；其他黑色手牌不妨碍使用，装备及木牛流马牌不计入该手牌集合。

## 实现

独立武将 `boundary:guan-ping`、`boundary` 变体、`sanguosha-ol` 规则集，龙吟引用与官方446完全同文的 `classic:longyin`，新增 `boundary:jieyong`。竭勇复用原生单手牌 view-as，使用与打出均走原来的杀或响应入口，保留普通距离、次数和目标限制；只在既有 `ProgramCardLegality` 注册两处实时门槛，限定本技能的实际唯一有效红色手牌。没有新增操作枚举、pending、付款或返回实现，也没有限定发动次数。

经典龙吟沿原生使用窗口、弃置手牌/装备、退还使用次数及红色摸牌；竭勇纯打出不构造使用账册，也不触发龙吟。行为检查覆盖唯一红手牌变化、原生使用与打出、龙吟已付款子窗冷恢复与红色收益；不重复静态配置快照或通用输入矩阵。

官方完整原画：[76600.png](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/big/static/76600.png)，815×1080，未裁改；`portraitKey` 为 `boundary-guan-ping`，素材由整批统一登记。
