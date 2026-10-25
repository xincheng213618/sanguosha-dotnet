# 公孙渊·移动版

核对日期：2026-10-26。身份 `mobile:gongsun-yuan`，群、男、4体力，一将成名2015（fame-5）。

[移动官网194](https://www.sanguosha.cn/pc/hero-detail-194.html)与[移动BWIKI人物当前页](https://wiki.biligame.com/msgs/公孙渊)（oldid27037）均明确要求展示的全部手牌含两种颜色，才能继续弃其中一种颜色的所有手牌并取得至多等量其他角色各一张牌；实际获得超过一张才失去1体力。单色仍可发动并展示，随后结束，本阶段次数已用。

使用独立 `mobile:huaiyi`，复用 `revealHandColorDiscardAndTake`，增加精确可选策略 `stateId: two-colors`。该策略依据原帧的已展示实体 roster 判定颜色，在原生展示子窗返回后才结束或提供费用选择；不改变原生真实弃牌、隐藏手牌不透明位置、至多X个不同目标、逐次取得实体及最终失体力的流程。所有已支付状态与恢复仍留在原 `HandControlDraft`。展示事件后重新读取当前原帧，以保留规则观察器同步追加的公开展示claims，避免旧帧覆盖回执；原生展示子窗返回后若单色无费用可选，则推进原程序完成。

版本依据存在分歧：[当前OL官网341](https://www.sanguosha.com/hero/341)的正文与库内 `classic:huaiyi` 一致，省略了双色门槛；[OL官方2015上架公告](https://www.sanguosha.com/news/20150917_2863_2609)和当前OL Wiki则列有该门槛。此次只按已明确的移动版规则新增可选策略，省略策略时保持库内经典版当前行为及其原有单色检查，不据此宣称OL版本历史已经厘清。

共享fixture新增移动版单色展示后无支付/无取得/无失HP，以及双色实际支付、暗手取得、取二失HP和代表冷恢复覆盖；不复制另一套完整人物夹具。

经典画像[官网34100](https://www.sanguosha.cn/storage/uploads/images/skins/34100.jpg)，资源键 `mobile-gongsun-yuan`，本地素材 `src/CardGame.Wpf/Assets/official-mobile-gongsun-yuan.jpg`。
