# 资源流套装（source 2230）

本条接入2026诸侯伐董当前内容。依据2026-10-21获取的公开 `tactics.json`、`tactic-tree.json` 与 `manifest.json` 固定快照；不以同品级其他战法价格推算本条价格。

| 字段 | 原始值 |
| --- | --- |
| source id | 2230 |
| 名称 | 资源流套装 |
| 效果原文 | 商店可无限刷新 |
| rarity | 4 |
| price | 9999 |
| seasonIds | 1、2、3、4、5、6、7 |
| deprecated | false |
| S5固定节点／随机节点引用 | 均无 |

来源端点为 [tactics.json](https://s.xiaochao.org/data/tactics.json)、[tactic-tree.json](https://s.xiaochao.org/data/tactic-tree.json)、[manifest.json](https://s.xiaochao.org/data/manifest.json)。核对使用上述日期的固定快照，不把端点未来内容视为同一版本。对应 SHA-256 分别为：

- `tactics.json`：`209ce819d57fc8985308376ecc24fe12d2c3ca94dec75b775578f34543774c0e`
- `tactic-tree.json`：`a754ca2f4ac25f0f140c8df2d97693b5bb84ec5edf55a52ac59f8cd511a18625`
- `manifest.json`：`d8563b00afac4d8ab692175771a4953ad1fdc8701cd46be4f3923714066e31d4`

## 套装判定与刷新流程

2026-10-26依据用户提供的实际组合截图和重新获取的同哈希`manifest.json`纠正此前投放：`schools`中的资源流明确以2230为组合效果，要求本局持有资源流普通2、稀有2、史诗2、传说1。原始`price=9999`字段不证明它可以作为单件商品购买。本地从商店和战后奖励候选移除2230，按组合条件派生刷新效果。

初始携带和局内实际获得的战法一起计数；仅研习解锁而未携带不计。依据来源的`schoolId`和实际品质填充对应格子，较高品质不能替代较低品质，同一来源的别名不重复填格。购买最后缺件后立即重新判断，战斗中也沿同一真实市场账本生效。

套装仅取消付费刷新次数限制。免费次数先用，随后仍按既有规则每次支付100铜币，余额不足不能刷新。普通一次、会员Ⅱ五次保持原约束；组合达成不重置已用次数、同类购买加价、余额或货架随机序号。快照以`ShopPaidRefreshUnlimited`表达无限，不用有限大整数冒充。

完整九套需求及当前版本效果见[S5流派套装](shanhe-school-combinations-2026-10-26.md)。旧的单件9999购买检查随错误取得流程一并替换，保留真实组合、免费优先、余额、会员、补货与品质提升相关机制验证。
