# 移动版界曹操

核对日期：2026-10-25。身份版，`boundary:cao-cao-mobile`，魏、男、4体力；角色变体 `boundary-mobile`，规则集 `sanguosha-mobile`。

## 来源与版本

- [三国杀移动版官网：界·曹操，hero164](https://www.sanguosha.cn/index.php/pc/hero-detail-164.html)：身份页同时完整列出奸雄和护驾。奸雄受伤后可同时获得伤害牌及摸一张，正文将获得放在摸牌之前。护驾是需要使用或打出闪时向其他魏势力角色征求闪的主公技，供应者打出的闪视为曹操使用或打出。
- [三国杀移动版 BWIKI：界曹操](https://wiki.biligame.com/msgs/%E7%95%8C%E6%9B%B9%E6%93%8D)（页面标记更新于2025-12-19）：身份栏补充4体力和男，并说明护驾依次征求其他魏势力角色。页面另列的国战、欢乐和1V1栏目不是此注册所用版本。

本仓库旧 `boundary:jianxiong` 只能二选一；曹婴授予的 `ol:jianxiong` 明确先摸再获得。两者均不能冒充官网当前移动版。新增 `boundary:jianxiong-mobile`，完整复用相同的 `boundary:hujia`，不复制其原生主公资格、供应者响应及实际使用/打出归属逻辑。

## 执行与边界

奸雄按每次伤害触发一次，不按伤害点逐次触发。先通过既有原生 `ClaimProgramDamageCards` 获得该原伤害对应、仍在处理区的全部实体牌，保留群体牌材料领取记录；完成获得牌所产生的原生移动子窗口后，才执行标准摸一张。牌在获得后的孩子里再移动，不取消已发行的摸牌。

没有实体伤害牌或牌已不在处理区时，获得部分为空，摸牌仍执行。这是按官网两个并列效果的执行顺序和引擎“尽可能执行”的既有效果合同采用的工程解释，不额外附加官网没有写的“有可获得实体牌”发动门槛。新薄操作只封装原生领取及原帧等待，避免旧的仅领取操作前置资格把整个技能拒绝。既有OL与经典定义保持原行为。

等待沿用原 `ProgramSkillFrame.PendingMovementContinuation` 和原生类型化移动返回。恢复证明绑定原伤害窗口、冻结候选中的技能实例、该原帧的领取事实，以及相同原帧和技能实例产生的真实移动账单；不以全游戏同牌领取次数为上限，也不以领取牌仍在手中为条件。获得子窗中失去原技能，已提交领取的原帧仍完成已发行的摸牌。没有新增并行pending、use-ID侧车、规则版本或特殊UI。

## 验证入口

新增行为检查：`Mobile boundary Cao Cao: native claim children precede draw and cardless damage cold`，方法 `BoundaryCaoCaoMobileChecks.NativeClaimChildrenPrecedeDrawAndCardlessDamageStillDrawsCold`。使用真实实体牌转化的原生决斗，在获得子窗中恢复并真实失去技能，再核对领取、摸牌与原材料支付均一次；通过共享授技恢复技能后，使用原生无实体伤害核对摸牌不会被旧领取资格阻止。

相反顺序的既有OL回归：`Cao Ying: Granted Current Jianxiong Draw Child Then Claims Native Damage Card`。护驾复用现有 `passive-response-rules` 的同一 `factionResponseRequest` 原生策略，不为相同配置新增重复夹具。共享供应者/实际使用归属回归可选 `Same name hand demand: actual faction providers own completion exactly once`；该检查是共同响应机制覆盖，并非把其角色特有额外效果当成移动护驾规则。
