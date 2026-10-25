# 夏侯氏（移动版）

核对日期：2026-10-25。真实普通移动身份为 `mobile:xiahou-shi`，蜀、女性、3体力，技能为樵拾、燕语；官网另列的谋夏侯氏属于不同版本，本次不虚构普通夏侯氏的“移动界”变体。

- [移动官网武将189](https://www.sanguosha.cn/index.php/pc/hero-detail-189.html)：核对全部隐藏正文及玩法说明。樵拾在其他角色的结束阶段，若双方手牌数相等，可让双方各摸一张；燕语在出牌阶段可以重铸杀，出牌阶段结束时，若本阶段重铸过至少两张杀，可以令一名男性角色摸两张。
- [移动版 BWIKI 夏侯氏](https://wiki.biligame.com/msgs/%E5%A4%8F%E4%BE%AF%E6%B0%8F)，页面更新于2025-11-27：核对同一普通移动身份及阶段内重铸门槛。未将谋版、OL界版的自己结束阶段、整轮禁用或仅失去杀的判据混入本版。

樵拾完整复用 `classic:qiaoshi`，保留“其他角色结束阶段”、当时实际相等手牌数、可选发动和双方各摸一张；不套用现有OL界 `boundary:qiaoshi-current` 的其他时点和限制。

燕语使用薄配置 `mobile:yanyu`，保留经典DSL的原生 `recastSelectedCards`，对本阶段实际重铸数量计数，以公开阶段布尔状态记录至少两次，阶段结束可选发动一次。它不是把杀当作使用或响应，也不把弃掉、转化使用或其他失去杀算为重铸。可重铸普通杀、火杀、雷杀、冰杀；经典旧数组缺少冰杀，因此没有直接别名旧技能。受益对象为任一存活男性，借得技能的男性本人同样合法，使用已有 `anyLivingMale`，不添加“其他角色”限制。

本人物没有新runtime、新pending或新事件，仅原生DSL配置。实际重铸付款、抽牌孩子、阶段计数及冷恢复继承已有机制检查，不复制人物夹具。相关既有过滤器包括 `Native recast invocation: single and whole group are distinct`、`Native recast invocation: two participants independent and cold`、`Native recast invocation: paid loss death and zero draw cold`，以及批次内容加载和图鉴检查。

立绘为[官网原画33600](https://www.sanguosha.cn/storage/uploads/images/skins/33600.jpg)，原尺寸530×750，不裁剪、不重采样；保存为 `official-mobile-xiahou-shi.jpg`。SHA-256：`bc9a0fc03dc3b64c3145d0a51c345d6e6c8ba8468fdec3cdb509ef8985ded88d`。
