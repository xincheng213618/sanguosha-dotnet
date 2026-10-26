# 常驻集市货品与原图

2026-10-26按用户提供的实际游玩截图调整集市：每类基础5件，商品扩展按实际持有数叠加；采用可翻页的常驻浮层。交易、即时生效与逐章货架约定见[常驻市场](shanhe-persistent-market.md)。

## 默认供货资格

下列奖励和原生程序早已实现，却未进入新征程默认供货名单，且S5研习树没有这些源条目的固定或随机指向。本批补齐供货资格，沿既有无节点奖励的本地约定加入候选，保留原价和技能槽规则。

| 来源 | 技能 | 原价 / 品质 | 程序 |
| --- | --- | --- | --- |
| 3011 | 强袭 | 600 / 3 | boundary:qiangxi |
| 3192 | 慎行 | 100 / 1 | classic:shenxing |
| 3016 | 连营 | 100 / 1 | boundary:lianying |
| 3259 | 狂骨 | 600 / 3 | classic:kuanggu |
| 3134 | 空城 | 100 / 1 | classic:kongcheng |
| 3065 | 业炎 | 300 / 2 | classic:yeyan |
| 3042 | 急救 | 300 / 2 | classic:jijiu |
| 3107 | 八阵 | 300 / 2 | classic:bazhen |
| 3207 | 极奢 | 300 / 2 | classic:jishe |
| 3015 | 胆守 | 300 / 2 | classic:danshou |
| 3273 | 备战 | 300 / 2 | ol:beizhan |
| 3025 | 武圣 | 300 / 2 | boundary:wusheng |

势斩、绝情、散谣、清弦和恢拓已有S5固定研习4、7、26、79、87，仍沿各自研习门槛。烈刃节点5也有真实奖励`codex-5`及`classic:lieren`原生程序。

## 图片来源与使用

原卡图使用既有完整[cards.json](https://s.xiaochao.org/data/cards.json)快照的`image.src`，对应快照SHA-256为`d699fc9524136ff4129a324187933fcec09cd1690f85481c51f2fa0a1f47e6cf`。战法图标使用2026-10-21固定[tactics.json](https://s.xiaochao.org/data/tactics.json)，SHA-256为`209ce819d57fc8985308376ecc24fe12d2c3ca94dec75b775578f34543774c0e`。

本批为现有已定价货品补齐52张缺失卡图和41张缺失战法图标，文件保存在`Assets/Shanhe/<来源ID>.png`。下载逐张核对PNG尺寸与资源地址中对应的SHA-256前缀。商店卡图优先使用该原图，花色点数仍由实际配方绘制；原有通用卡图仅在没有源图时回退。

- 刑天破军斧4059已有完整原图：[资源](https://s.xiaochao.org/data/images/cf5ba6deb710b3b3c9a63a92.png)。
- 无双方天戟4026本批补齐完整原图：[资源](https://s.xiaochao.org/data/images/0ecbb32fc66c2c3be8c682a2.png)。

正文展示核心效果；材料只显示“获得卡牌”，完整实际规则放在悬停说明。来源、实现和重复取得流程说明留在开发文档及玩法说明中。
