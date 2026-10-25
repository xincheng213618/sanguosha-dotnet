# 王异·移动版（普通身份）

核对日期：2026-10-25。

- [移动版官网王异完整图鉴（112）](https://www.sanguosha.cn/pc/hero-detail-112.html)：魏、女、3体力，读取含隐藏技能项的完整页面。
- [移动版 BWIKI 普通王异](https://wiki.biligame.com/msgs/%E7%8E%8B%E5%BC%82)：页面标示2026-01-09更新，普通“经典”栏与官网一致，明确自改判、准备或结束阶段判黑、看顶牌后全交给一名角色。

此普通移动版采用原始贞烈、秘计：

- 贞烈：自己的判定牌生效前，可以亮出牌堆顶一张牌，打出并代替当前判定牌。玩家在看到替换牌之前决定发动；只能改自己的判定，不能挑选手牌或预览未知顶牌。原判定牌置入弃牌堆，真实新顶牌经过原生Processing进入同一个判定区，由原判定继续结算。
- 秘计：准备或结束阶段受伤时，可以进行一次判定；最终判定结果为黑色，则私下观看牌堆顶等于已损失体力值张数的牌，然后将这些牌全部交给一名角色。可以选自己；不逐张分给多人，不获得其他手牌的自由分配权，也不是摸牌事件。红色结果不看牌、不要求选目标。

没有复用仓库 `classic:zhenlie` / `classic:miji` 或界王异，因为这些是“成为杀或锦囊目标后失去体力免牌、结束阶段摸牌再分手牌”的不同版本。[官网攻略46](https://www.sanguosha.cn/pc/guide-info-46.html)明确讲“新王异”，不能覆盖官网112及当前普通移动Wiki仍保留的原始版。检索官方后续调整，未发现针对此原始普通移动身份的替代公告。

实现使用 `mobile:zhenlie`、`mobile:miji`。贞烈扩展已有 `replaceJudgment` 的独占 `drawPile` 输入，确认发动前只显示无牌ID的发动/放弃按钮；确认后冻结当前真实顶牌，仍由原 `JudgmentFrame` 与 `ProgramSkillFrame.WindowContext.JudgmentReplacement` 支付和返回。原手牌/装备/公共米堆的改判入口保持原契约。

秘计复用 `startJudgment` 的冻结 `outcomeBind`，清理尚未被取得的判定牌，再由同一黑色结果条件执行 `revealTopCards(private, ownerLostHp)`、`selectTarget(anyLiving)`、`moveBoundCards(selectedTargetHand)`。静态组合验证将临时牌与目标绑定在产生它们的同一结果分支；看牌和给牌均使用原生移动，真实给牌后的子结算在原帧上返回，不重新支付或抽取顶牌。

新增行为检查 `MobileWangYiChecks.UnknownTopRetrialAndWholePrivateGiftReturnCold`，覆盖未知顶牌确认、自身判定替换、红结果无礼物、黑结果全部给一人、结束/准备两个实际阶段入口及原移动孩子冷恢复。纯复用的共有子流程不另建场景矩阵。

肖像为官网原始经典立绘 `https://www.sanguosha.cn/storage/uploads/images/skins/16300.jpg`，保留原始530×750图片。
