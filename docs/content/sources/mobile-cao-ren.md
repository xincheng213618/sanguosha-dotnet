# 曹仁·移动版

核查日期：2026-10-26。普通移动身份，魏、男、4体力、神话再临风包；独立 `mobile:jushou` 编排既有摸牌与双向翻面操作，没有新增运行时。

来源：[移动官网29](https://www.sanguosha.cn/pc/hero-detail-29.html)、[移动BWIKI人物34822](https://wiki.biligame.com/msgs/index.php?title=曹仁&oldid=34822)（2025-12-05）、[普通据守20562](https://wiki.biligame.com/msgs/index.php?title=据守/曹仁&oldid=20562)（2023-01-03）。当前经典栏给出50级获取；这是已上线的普通风包身份，不混用界版摸4及其后弃牌/使用装备。

官网与移动Wiki均明确结束阶段摸3后翻面。现有 `classic:jushou` 实际先摸3，再 `setFaceState(faceDown:true)`，已处于背面的曹仁仍保持背面，不是双向翻面。合法的本人Play阶段翻背再进入Ending场景已由现有 `TurnEndingBoundaryChecks.OrdersJushouJujianBiyueAndReplays` 建立，不能据经典旧合同宣称当前移动普通版完整等价。

移动版仅用独立JSON执行 `draw:3` 后 `turnOver`。原生摸牌实体、获得子窗和typed返回仍归原据守帧，子窗返回后执行一次翻面；正面变背面，背面变正面。没有固定背面效果，没有改经典旧定义，也没有把两步换成手牌数调整或合成翻面事实。

差异focused检查用真实Play翻背的前置状态，进入Ending后发动普通据守，暂停在真实摸3的获得子窗并cold，随后返回原据守帧翻正一次，保留三张原摸牌实体与顺序断言。本次构建和统一定向由主代理执行。

普通经典图：[2900.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2900.jpg)，为官网29首图。`portraitKey` 为 `mobile-cao-ren`，图鉴分组 `myth-wind`，技能85000、人物/将池85099。
