
package avatar.handler;

import avatar.constants.Cmd;
import avatar.constants.NpcName;
import avatar.item.Item;
import avatar.model.*;

import java.util.*;
import java.math.BigInteger;

import avatar.lucky.DialLucky;
import avatar.lucky.DialLuckyManager;

import java.util.concurrent.CountDownLatch;
import java.io.IOException;
import java.util.concurrent.*;
import avatar.network.Message;
import avatar.play.MapManager;
import avatar.play.NpcManager;
import avatar.play.Zone;
import avatar.server.ServerManager;
import avatar.server.UserManager;
import avatar.server.Utils;
import avatar.service.AvatarService;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.concurrent.CountDownLatch;

import static avatar.constants.NpcName.*;
import static avatar.constants.NpcName.boss;
import java.io.IOException;
import java.util.HashMap;
import java.util.List;


import java.time.LocalTime;

public class NpcHandler {

    private static final Map<Integer, Long> lastActionTimes = new HashMap<>();
    private static final long ACTION_COOLDOWN_MS = 90; // 2 giây cooldown


    public static void handleDiaLucky(User us, byte type) {
        DialLucky dl = DialLuckyManager.getInstance().find(type);

        if (dl != null) {
            if (dl.getType() == DialLuckyManager.MIEN_PHI) {
                if(us.chests.size() >= us.getChestSlot()-2){
                    us.getAvatarService().serverDialog("Bạn phải có ít nhất 3 ô trống trong rương đồ");
                    return;
                }
                Item itm = us.findItemInChests(593);
                if (itm == null || itm.getQuantity() <= 0) {
                    us.getAvatarService().serverDialog("Bạn không có Vé quay số miễn phí!");
                    return;
                }
            }
            if (dl.getType() == DialLuckyManager.XU) {
                if(us.chests.size() >= us.getChestSlot()-2){
                    us.getAvatarService().serverDialog("Bạn phải có ít nhất 3 ô trống trong rương đồ");
                    return;
                }
                if (us.getXu() < 15000) {
                    us.getAvatarService().serverDialog("Bạn không đủ xu!");
                    return;
                }
            }
            if (dl.getType() == DialLuckyManager.LUONG) {
                if(us.chests.size() >= us.getChestSlot()-2){
                    us.getAvatarService().serverDialog("Bạn phải có ít nhất 3 ô trống trong rương đồ");
                    return;
                }
                if (us.getLuong() < 5) {
                    us.getAvatarService().serverDialog("Bạn không đủ lượng!!");
                    return;
                }
            }
        }
        us.setDialLucky(dl);
        dl.show(us);
    }

    public static void handlerCommunicate(int npcId, User us) throws IOException {
        Zone z = us.getZone();
        if (z != null) {
            User u = z.find(npcId);
            if (u == null) {
                return;
            }
        } else {
            return;
        }

        long currentTime = System.currentTimeMillis();
        long lastActionTime = lastActionTimes.getOrDefault(us.getId(), 0L);

        if (currentTime - lastActionTime < ACTION_COOLDOWN_MS) {
            us.getAvatarService().serverDialog("Từ từ thôi bạn!");
            return;
        }
        // Cập nhật thời gian thực hiện hành động
        lastActionTimes.put(us.getId(), currentTime);

        int npcIdCase = npcId - Npc.ID_ADD;
        User boss = z.find(npcId);

        double maxDistance = 55.0;
        int playerX = us.getX();
        int playerY = us.getY();
        int bossX = boss.getX();
        int bossY = boss.getY();
        double distance = Utils.distanceBetween(playerX, playerY, bossX, bossY);


        if (npcIdCase > 1000 && npcIdCase<=9999)
        {

            if (boss.isDefeated()) {
                us.getAvatarService().serverDialog("boss đã chết");
                return;
            }
            if (distance > maxDistance) {
                us.getAvatarService().serverDialog("Bạn đứng xa rồi : v");
                return;
            }
            LocalTime now = LocalTime.now();

            // Đặt khoảng thời gian hợp lệ
            LocalTime startTime = LocalTime.of(6, 0); // 6h sáng
            LocalTime endTime = LocalTime.of(23, 59);  // 11h đêm

            // Kiểm tra nếu thời gian hiện tại nằm trong khoảng
            if (now.isAfter(startTime) && now.isBefore(endTime)) {
                us.updateXuKillBoss(+us.getDameToXu());
            } else {
                // Xử lý nếu thời gian không nằm trong khoảng
                System.out.println("Hàm không được kích hoạt ngoài khoảng thời gian từ 6h sáng đến 11h đêm.");
            }



            us.updateXu(+us.getDameToXu());

            us.getAvatarService().updateMoney(0);

            List<User> lstUs = us.getZone().getPlayers();

            if (us.findItemInWearing(3174)!=null){
                us.skillUidToBoss(lstUs,us.getId(),npcId,(byte)25,(byte)26);
                boss.updateHP(-us.getDameToXu(),(Boss)boss, us);
            }else if (us.findItemInWearing(4715)!=null) {
                us.skillUidToBoss(lstUs,us.getId(),npcId,(byte)38,(byte)39);
                boss.updateHP(-us.getDameToXu(),(Boss)boss, us);
            }else {
                us.skillUidToBoss(lstUs,us.getId(),npcId,(byte)23,(byte)24);
                boss.updateHP(-us.getDameToXu(),(Boss)boss, us);
            }

        } else if (npcIdCase >= 10000) {
            if (distance > maxDistance) {
                us.getAvatarService().serverDialog("Bạn đứng xa rồi : v");
                return;
            }
            if(boss.isSpam()){
                us.getAvatarService().serverDialog("hộp này đã nhặt");
                return;
            }
            us.updateSpam(-1,(Boss)boss,us);



        }else {
            switch (npcIdCase) {
                case NpcName.Tien_chi_mu_Lovanga:
                    List<Menu> list = new ArrayList<>();
                    Menu tienchi = Menu.builder().name("Dự đoán vị trí người yêu").action(() -> {
                        us.getService().serverDialog(us.getService().DuDoanNY(us));
                                            }).build();
                    list.add(tienchi);
                    list.add(Menu.builder().name("Dự đoán kết quả sổ số").action(() -> {
                        us.getService().serverDialog("Dự đoán kết quả : Comingsion");
                    }).build());
                    list.add(Menu.builder().name("Dự đoán kết quả Tài Xỉu").action(() -> {
                        us.getService().serverDialog("Dự đoán kết quả : Comingsion");
                    }).build());
                    list.add(Menu.builder().name("Thoát").build());
                    us.setMenus(list);
                    us.getAvatarService().openMenuOption(npcId, 0, list);
                    break;
                case NpcName.bunma:
                    List<Menu> list1 = new ArrayList<>();
                    Menu Event = Menu.builder().name("Đổi Quà").action(() -> {
                        ShopEventHandler.displayUI(us, bunma,2034,2035,2036,2040,3506,2620,2577,5539,2618, 2619, 3987,3455,3456,3457,4995,6772,6773,6774);
                    }).build();
                    list1.add(Event);
                    list1.add(Menu.builder().name("Góp Ngọc Rồng Lỏ")
                            .action(() -> {
                                GopDiemSK(us);
                            })
                            .build());
                    list1.add(Menu.builder().name("Thành tích bản thân")
                            .action(() -> {
                                StringBuilder detailedMessage = new StringBuilder("Thành tích bản thân");
                                detailedMessage.append(String.format("\n Bạn đang có %d điểm sự kiện", us.getScores()));
                                int rankPhaoLuong = us.getService().getUserRankPhaoLuong(us);
                                detailedMessage.append(String.format("\n Bạn đang ở top %d thả pháo lượng : %d", rankPhaoLuong,us.getTopPhaoLuong()));

                                int rankXuboss = us.getService().getUserRankXuBoss(us);
                                detailedMessage.append(String.format("\n Bạn đang ở top %d xu boss : %d", rankXuboss, us.getXu_from_boss()));
                                us.getAvatarService().serverDialog(detailedMessage.toString());
                            })
                            .build());
                    list1.add(Menu.builder().name("Bảng xếp hạng kiếm xu từ boss")
                            .action(() -> {
                                List<User> topPlayers = us.getService().getTop10PlayersByXuFromBoss();
                                StringBuilder result = new StringBuilder();
                                int rank = 1; // Biến đếm để theo dõi thứ hạng

                                for (User player : topPlayers) {
                                    if (player.getXu_from_boss() > 0) {
                                        result.append(player.getUsername())
                                                .append(" Top ").append(rank).append(" : ")
                                                .append(player.getXu_from_boss())
                                                .append(" xu\n");
                                        rank++; // Tăng thứ hạng sau mỗi lần thêm người chơi vào kết quả
                                    }
                                }

                                us.getAvatarService().customTab("Top 10", result.toString());
                            })
                            .build());
                    list1.add(Menu.builder().name("Bảng xếp hạng thả pháo lượng")
                            .action(() -> {
                                List<User> topPlayers = us.getService().getTopPhaoLuong();
                                StringBuilder result = new StringBuilder();
                                int rank = 1; // Biến đếm để theo dõi thứ hạng

                                for (User player : topPlayers) {
                                    if (player.getTopPhaoLuong() > 0) {
                                        result.append(player.getUsername())
                                                .append(" Top ").append(rank).append(" : ")
                                                .append(player.getTopPhaoLuong())
                                                .append("\n");
                                        rank++; // Tăng thứ hạng sau mỗi lần thêm người chơi vào kết quả
                                    }
                                }
                                us.getAvatarService().customTab("Top 10", result.toString());
                            })
                            .build());
                    list1.add(Menu.builder().name("Xem hướng dẫn")
                            .action(() -> {
                                us.getAvatarService().customTab("Hướng dẫn", "-Từ Ngày");
                            })
                            .build());
                    list1.add(Menu.builder().name("Thoát").id(npcId).build());
                    us.setMenus(list1);
                    us.getAvatarService().openMenuOption(npcId, 0, list1);
                    break;
                case NpcName.Vegeta:
                    List<Menu> lstVegeta = new ArrayList<>();


                    Menu vegenta = Menu.builder().name("Quà Thẻ VIP PREMIUM").action(() -> {
                        ShopEventHandler.displayUI(us, Vegeta,5822,6450,6314);
                    }).build();
                    lstVegeta.add(Menu.builder().name("Quà Thẻ VIP Cao Cấp").action(() -> {
                        ShopEventHandler.displayUI(us, Vegeta, 6113,6553);
                    }).build());
                    lstVegeta.add(vegenta);

                    lstVegeta.add(Menu.builder().name("Quà Thẻ VIP").action(() -> {
                        ShopEventHandler.displayUI(us, Vegeta, 3638 , 3636, 620,2090,6541,2052,2053,3636,3638);
                    }).build());

                    lstVegeta.add(Menu.builder().name("Xem hướng dẫn")
                            .action(() -> {
                                us.getAvatarService().customTab("Hướng dẫn", "từ hôm nay đến hết 04 tháng 10 năm 2024 TOP 5 thả lượng sẽ được set Xên Bọ Hung Vĩnh Viễn(120 dame)  ở TOP 3 thì thêm 1 Thẻ Vip Cao Cấp, riêng TOP 1 nhận thêm thẻ VIP PREMIUM, TOP 5 XU từ BOSS Sẽ nhận được tóc Tóc Siêu Xaya cấp 2 TOP 3 XU Từ boss sẽ được 1 thẻ VIP, và 1 set Super Saiyan Rose(120 dame) riêng top 1 thêm tóc Super Saiyan Blue 6");
                            })
                            .build());
                    lstVegeta.add(Menu.builder().name("Thoát").id(npcId).build());
                    us.setMenus(lstVegeta);

                    us.getAvatarService().openMenuOption(npcId, 0, lstVegeta);
                    break;
                case NpcName.CHU_DAU_TU:
                    break;
//                case NpcName.em_Thinh:{
//                    List<Menu> listet = new ArrayList<>();
//                    List<Item> Items = new ArrayList<>();
//                    Menu quaySo = Menu.builder().name("vật phẩm").menus(
//                                    List.of(
//                                            Menu.builder().name("demo item").action(() -> {
//                                                for (int i = 2000; i < 6676; i++) {
//                                                    Item item = new Item((short) i);
//                                                    Items.add(item);
//                                                }
//                                                us.getAvatarService().openUIShop(-49,"em.Thinh",Items);
//                                            }).build()
//                                    ))
//                            .id(npcId)
//                            .npcName("donate đi")
//                            .npcChat("show Item")
//                            .build();
//                    listet.add(quaySo);
//                    listet.add(Menu.builder().name("Hướng dẫn").action(() -> {
//                        us.getAvatarService().customTab("Hướng dẫn", "hãy nạp lần đầu để mở khóa mua =)))");
//                    }).build());
//                    listet.add(Menu.builder().name("Thoát").build());
//                    us.setMenus(listet);
//                    us.getAvatarService().openUIMenu(npcId, 0, listet, "donate đi", "");
//                    break;
//                }
                case NpcName.QUAY_SO: {
                    List<Menu> qs = new ArrayList<>();
                    Menu quaySo1 = Menu.builder().name("Quay số").menus(
                                    List.of(
                                            Menu.builder().name("5 lượng").action(() -> {
                                                System.out.println("Action for 5 lượng triggered");
                                                handleDiaLucky(us, DialLuckyManager.LUONG);
                                            }).build(),
                                            Menu.builder().name("25.000 xu").action(() -> {
                                                System.out.println("Action for 15.000 xu triggered");
                                                handleDiaLucky(us, DialLuckyManager.XU);
                                            }).build(),
                                            Menu.builder().name("Q.S miễn phí").action(() -> {
                                                System.out.println("Action for Q.S miễn phí triggered");
                                                handleDiaLucky(us, DialLuckyManager.MIEN_PHI);
                                            }).build(),
                                            Menu.builder().name("Thoát").action(() -> {
                                                System.out.println("Exit menu triggered");
                                            }).build()
                                    ))
                            .id(npcId)
                            .build();
                    qs.add(quaySo1);
                    qs.add(Menu.builder().name("Xem hướng dẫn").action(() -> {
                        System.out.println("Action for Xem hướng dẫn triggered");
                        us.getAvatarService().customTab("Hướng dẫn", "Để tham gia quay số bạn phải có ít nhất 5 lượng hoặc 25 ngàn xu trong tài khoản và 3 ô trống trong rương\n Bạn sẽ nhận được danh sách những món đồ đặc biệt mà bạn muốn quay. Những món đồ đặc biệt này bạn sẽ không thể tìm thấy trong bất cứ shop nào của thành phố.\n Sau khi chọn được món đồ muốn quay bạn sẽ bắt đầu chỉnh vòng quay để quay\n Khi quay bạn giữ phím 5 để chỉnh lực quay sau đó thả ra để bắt đầu quay\n Khi quay bạn sẽ có cơ hội trúng từ 1 đến 3 món quà\n Quà của bạn nhận được có thể là vật phẩm bất kì, xu, hoặc điểm kinh nghiệm\n Bạn có thể quay được những bộ đồ bán bằng lượng như đồ hiệp sĩ, pháp sư...\n Tuy nhiên vật phẩm bạn quay được sẽ có hạn sử dụng trong một số ngày nhất định.\n Nếu bạn quay được đúng món đồ mà bạn đã chọn thì bạn sẽ được sở hữu món đồ đó vĩnh viễn.\n Hãy thử vận may để sở hữa các món đồ cực khủng nào !!!");
                    }).build());
                    qs.add(Menu.builder().name("Thoát").build());
                    us.setMenus(qs);
                    us.getAvatarService().openMenuOption(npcId, 0, qs);
                    break;
                }
                case NpcName.THO_KIM_HOAN: {
                    List<Menu> nangcap = new ArrayList<>();
                    String npcName = "Thợ KH";
                    String npcChat = "Muốn nâng cấp đồ thì vào đây";
                    Menu upgrade = Menu.builder().name("Nâng cấp").id(npcId).npcName(npcName).npcChat(npcChat).menus(
                                    List.of(
                                            Menu.builder().name("Nâng cấp xu").id(npcId).npcName(npcName).npcChat(npcChat)
                                                    .menus(listItemUpgrade(npcId, us, BossShopHandler.SELECT_XU))
                                                    .build(),
                                            Menu.builder().name("Nâng cấp lượng").id(npcId).npcName(npcName).npcChat(npcChat)
                                                    .menus(listItemUpgrade(npcId, us, BossShopHandler.SELECT_LUONG))
                                                    .id(npcId)
                                                    .build(),
                                            Menu.builder().name("Thoát").id(npcId).build()
                                    )
                            )
                            .build();
                    nangcap.add(upgrade);
                    nangcap.add(Menu.builder().name("Xem hướng dẫn")
                            .action(() -> {
                                us.getAvatarService().customTab("Hướng dẫn", "Nâng thì nâng không nâng thì cút!");
                            })
                            .build());
                    nangcap.add(Menu.builder().name("Thoát").id(npcId).build());
                    us.setMenus(nangcap);
                    us.getAvatarService().openUIMenu(npcId, 0, nangcap, npcName, npcChat);
                    break;
                }
                case NpcName.LAI_BUON: {
                    List<Menu> laibuon = new ArrayList<>();
                    Menu LAI_BUON = Menu.builder().name("Điểm Danh").action(() -> {
                        Item item = new Item(593, -1, 1);
                        //us.addItemToChests(item);
                        //us.addExp(5);
                        us.getService().serverMessage("đang xây dựng");//Bạn nhận được 5 điểm exp + 1 thẻ quay số miễn phí");
                    }).build();
                    laibuon.add(LAI_BUON);
                    laibuon.add(Menu.builder().name("Xem hướng dẫn").action(() -> {
                        us.getAvatarService().customTab("Hướng dẫn", "Đăng nhập mỗi ngày để nhận quà.\nDùng điểm chuyên cần để nhận đucợ những món quà có giá trị trong tương lai");
                    }).build());
                    laibuon.add(Menu.builder().name("Thoát").build());
                    us.setMenus(laibuon);
                    us.getAvatarService().openMenuOption(npcId, 0, laibuon);
                    break;
                }
                case NpcName.THO_CAU:
                    List<Menu> thocau = new ArrayList<>();
                    Menu thoCau = Menu.builder().name("Câu cá").action(() -> {
                        List<Item> Items1 = new ArrayList<>();
                        Item item = new Item(446,30,0);//câu vip
                        Items1.add(item);
                        Item item1 = new Item(460,2,0);//vé cau
                        Items1.add(item1);
                        Item item2 = new Item(448,30,1);//mồi
                        Items1.add(item2);
                        us.getAvatarService().openUIShop(npcId,"Trùm Câu Cá,",Items1);
                        us.getAvatarService().updateMoney(0);
                    }).build();
                    thocau.add(thoCau);
                    thocau.add(Menu.builder().name("Bán cá").action(() -> {
                        try {
                            sellFish(us);
                        } catch (IOException e) {
                            throw new RuntimeException(e);
                        }
                    }).build());
                    thocau.add(Menu.builder().name("Xem hướng dẫn").action(() -> {
                        us.getAvatarService().customTab("Hướng dẫn", "Câu cá kiếm được nhiều xu bản auto lên thanhpholo.com");
                    }).build());
                    thocau.add(Menu.builder().name("Thoát").build());
                    us.setMenus(thocau);
                    us.getAvatarService().openMenuOption(npcId,0,  thocau);
                    break;
            }
        }
    }

    public static void sellFish(User us) throws IOException {
        int[] array = {2130,2131,2132,454,455,456,457};

        for (int idFish : array) {
            Item item = us.findItemInChests(idFish); // Tìm item trong rương theo idFish

            // Nếu không tìm thấy item, tiếp tục với ID tiếp theo
            while (item != null && item.getQuantity() > 0) {
                int sellPrice = item.getPart().getCoin(); // Giá bán 1 món đồ
                String message = String.format("Bạn vừa bán 1 %s với giá = %d xu.", item.getPart().getName(), sellPrice);

                us.removeItemFromChests(item);
                us.updateXu(sellPrice); // Cập nhật xu

                us.getAvatarService().updateMoney(0); // Cập nhật tiền tệ
                us.getAvatarService().SendTabmsg(message); // Gửi thông báo bán hàng

                // Cập nhật lại item để kiểm tra số lượng
                item = us.findItemInChests(idFish);
            }
        }
    }

    public static void GopDiemSK(User us){
        java.util.Map<Integer, Integer> itemsToProcess = new HashMap<>();
        itemsToProcess.put(4081, 1);
        itemsToProcess.put(4082, 2);
        itemsToProcess.put(4083, 3);
        itemsToProcess.put(4084, 4);
        itemsToProcess.put(4085, 5);
        itemsToProcess.put(4086, 6);
        itemsToProcess.put(4087, 7);
        int addscores = 0;
// Lặp qua từng cặp ID và số lượng

        StringBuilder detailedMessage = new StringBuilder("Bạn đã đổi thành công:");
        for (java.util.Map.Entry<Integer, Integer> entry : itemsToProcess.entrySet()) {
            int itemId = entry.getKey();
            int scores = entry.getValue();
            Item item = us.findItemInChests(itemId);
            if (item != null && item.getQuantity() > 0) {
                addscores += item.getQuantity()*scores;
                detailedMessage.append(String.format("\n%s :(Điểm %d) Số lượng %d  x  tong %d điểm", item.getPart().getName(), scores, item.getQuantity(), item.getQuantity()*scores));
                us.updateScores(+addscores);
                us.removeItem(itemId, item.getQuantity());
            }
        }
        if(addscores > 0){
            detailedMessage.append(String.format("\n Tổng tất cả %d",addscores) +" điểm");
            us.getAvatarService().serverDialog(detailedMessage.toString());
        }else {
            us.getAvatarService().serverDialog("Bạn không còn ngọc rồng");
        }
    }

    public static List<Menu> listItemUpgrade(int npcId, User us, byte type) {
        //String npcName = "Thợ KH";
        //String npcChat = "Muốn đồ đang mặc đẹp hơn không? Ta có thể giúp bạn đấy";
        return List.of(
                Menu.builder().name("Quà cầm tay").id(npcId)
                        .menus(List.of(
                                        Menu.builder().name("Bông hoa cổ tích").action(() -> {
                                            BossShopHandler.displayUI(us, type, 6212, 6213, 6214);
                                        }).build(),
                                        Menu.builder().name("Hoa hồng phong thần").action(() -> {
                                            BossShopHandler.displayUI(us, type, 5321, 5322, 5323);
                                        }).build(),
                                        Menu.builder().name("Hoa hồng xanh pha lê thần thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 5286, 5287, 5288);
                                        }).build(),
                                        Menu.builder().name("Mộc thảo hồ điệp").action(() -> {
                                            BossShopHandler.displayUI(us, type, 4160, 4161, 4162, 4163, 5050);
                                        }).build(),
                                        Menu.builder().name("Cung thần tình yêu thần thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 4893, 4894, 4895);
                                        }).build(),
                                        Menu.builder().name("Cung xanh thần thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 4890, 4891, 4892);
                                        }).build(),
                                        Menu.builder().name("Gậy thả thính mê hoặc").action(() -> {
                                            BossShopHandler.displayUI(us, type, 3507, 4218);
                                        }).build(),
                                        Menu.builder().name("Chong chóng thiên thần").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2238, 2239, 2274, 2275, 2404);
                                        }).build(),
                                        Menu.builder().name("Cục vàng huyền thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2217, 2218, 2219, 2220, 2221, 2222, 2223);
                                        }).build()

                                )
                        )
                        .build(),
                Menu.builder().name("Nón").menus(List.of(
                                Menu.builder().name("Nón phù thuỷ hoả ngục truyền thuyết").action(() -> {
                                    BossShopHandler.displayUI(us, type, 2411, 2412, 2413, 2414, 5503, 5504);
                                }).build(),
                                Menu.builder().name("Vương miện hoàng thân").action(() -> {
                                    BossShopHandler.displayUI(us, type, 5394);
                                }).build(),
                                Menu.builder().name("Vương miện hoàng thân").action(() -> {
                                    BossShopHandler.displayUI(us, type, 5391);
                                }).build(),
                                Menu.builder().name("Tôi thấy hoa vàng trên cỏ xanh").action(() -> {
                                    BossShopHandler.displayUI(us, type, 3266, 3267, 3268, 3269, 3954);
                                }).build(),
                                Menu.builder().name("Vương miện phép màu").action(() -> {
                                    BossShopHandler.displayUI(us, type, 3422, 3423, 3639, 3640);
                                }).build(),
                                Menu.builder().name("Mũ ảo thuật tinh anh").action(() -> {
                                    BossShopHandler.displayUI(us, type, 2899, 2900, 2901, 2902, 2903, 3037, 3038, 3039);
                                }).build(),
                                Menu.builder().name("Cửu vỹ hồ ly").action(() -> {
                                    BossShopHandler.displayUI(us, type, 4724, 4728, 4729);
                                }).build(),
                                Menu.builder().name("Vương miện huyền vũ").action(() -> {
                                    BossShopHandler.displayUI(us, type, 2997, 2998, 2999);
                                }).build()
                        ))
                        .build(),
                Menu.builder().name("Trang phục")
                        .menus(
                                List.of(
                                        Menu.builder().name("Danh gia vọng tộc").action(() -> {
                                            BossShopHandler.displayUI(us, type, 5392, 5393);
                                        }).build(),
                                        Menu.builder().name("Nữ hoàng sương mai").action(() -> {
                                            BossShopHandler.displayUI(us, type, 5054, 5055);
                                        }).build(),
                                        Menu.builder().name("Bá tước bóng đêm").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2876, 2877);
                                        }).build(),
                                        Menu.builder().name("Napoleon").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2231, 2232);
                                        }).build(),
                                        Menu.builder().name("Elizabeth").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2229, 2230);
                                        }).build()
                                )
                        )
                        .build(),
                Menu.builder().name("Cánh")
                        .menus(
                                List.of(
                                        Menu.builder().name("Cánh tiểu thần phong linh").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2419, 2482, 2483, 2505, 2506, 5252, 5253);
                                        }).build(),
                                        Menu.builder().name("Cửu vỹ hồ ly thần thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 4333, 4910, 4911, 4912, 4913, 4914, 4915, 4916, 4334, 4889);
                                        }).build(),
//                                        Menu.builder().name("Cánh băng hoả thần thoại").action(() -> {
//                                            BossShopHandler.displayUI(us, type, 3448, 4057, 4375);
//                                        }).build(),
//                                        Menu.builder().name("Cánh hoả thần").action(() -> {
//                                            BossShopHandler.displayUI(us, type, 4311, 4312, 4313);
//                                        }).build(),
                                        Menu.builder().name("Cánh thiên sứ tình yêu").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2148, 2149, 2150, 2151, 2152, 3637);
                                        }).build(),
                                        Menu.builder().name("Cánh thiên sứ").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2142, 2143, 2144, 2145, 2146, 3635);
                                        }).build(),
                                        Menu.builder().name("Cánh địa ngục hắc ám").action(() -> {
                                            BossShopHandler.displayUI(us, type, 3529, 3530, 3531, 3532);
                                        }).build(),
                                        Menu.builder().name("Cánh cổng địa ngục").action(() -> {
                                            BossShopHandler.displayUI(us, type, 3522, 3523, 3524, 3525, 3526, 3527);
                                        }).build(),
//                                        Menu.builder().name("Cánh bướm đêm huyền thoại").action(() -> {
//                                            BossShopHandler.displayUI(us, type, 3366, 3379);
//                                        }).build(),
                                        Menu.builder().name("Cánh băng giá huyền thoại").action(() -> {
                                            BossShopHandler.displayUI(us, type, 3365, 3378);
                                        }).build(),
                                        Menu.builder().name("Cánh phép màu ước mơ").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2793, 2794, 2795, 2796);
                                        }).build(),
                                        Menu.builder().name("Cánh blue vững vàng").action(() -> {
                                            BossShopHandler.displayUI(us, type, 2788, 2789, 2790, 2791);
                                        }).build()
                                )
                        )
                        .build(),
                Menu.builder().name("Thú cưng")
                        .menus(List.of(
//                                Menu.builder().name("Lang thần lãnh nguyên").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 5517, 5518);
//                                }).build(),
//                                Menu.builder().name("Thiên thần hồ điệp").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 5486, 5487);
//                                }).build(),
                                Menu.builder().name("Thiên thần hộ mệnh toàn năng").action(() -> {
                                    BossShopHandler.displayUI(us, type, 5224, 5225, 5226);
                                }).build(),
//                                Menu.builder().name("Tiểu tiên bướm").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4305, 5058);
//                                }).build(),
//                                Menu.builder().name("Cáo tuyết cửu vỹ").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4904, 4905);
//                                }).build(),
//                                Menu.builder().name("Ma vương").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4096, 4731);
//                                }).build(),
//                                Menu.builder().name("Cửu vỹ hồ ly").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4724, 4728, 4729);
//                                }).build(),
//                                Menu.builder().name("Lợn lém lỉnh").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4376);
//                                }).build(),
//                                Menu.builder().name("Tuần lộc tinh anh").action(() -> {
//                                    BossShopHandler.displayUI(us, type, 4323, 4324);
//                                }).build(),
                                Menu.builder().name("Bay nax 2.0").action(() -> {
                                    BossShopHandler.displayUI(us, type, 4079, 4080);
                                }).build(),
                                Menu.builder().name("Phương hoàng lửa").action(() -> {
                                    BossShopHandler.displayUI(us, type, 3668, 3771, 3772, 3773, 3854);
                                }).build(),
                                Menu.builder().name("King Kong").action(() -> {
                                    BossShopHandler.displayUI(us, type, 3744);
                                }).build(),
                                Menu.builder().name("Kỳ lân truyền thuyết").action(() -> {
                                    BossShopHandler.displayUI(us, type, 2726, 2727, 2728, 2729, 2730);
                                }).build()
                        ))
                        .build()
        );
    }

    public static void handlerAction(User us, int npcId, byte menuId, byte select) throws IOException {
        Zone z = us.getZone();
        if (z != null) {
            User u = z.find(npcId);
            if (u == null) {
                return;
            }
        } else {
            return;
        }
//        if (menuId == 0 && select == 0) {
//            // Trường hợp đặc biệt khi lần đầu mở menu
//            System.out.println("Initial menu open, displaying options without performing action.");
//            us.getAvatarService().openMenuOption(npcId, menuId,us.getMenus());
//            return;
//        }
//        int npcIdCase = npcId - 2000000000;
        List<Menu> menus = us.getMenus();
        if (menus != null && select < menus.size()) {
            Menu menu = menus.get(select);
            if (menu.isMenu()) {
                us.setMenus(menu.getMenus());
                us.getAvatarService().openUIMenu(npcId, menuId + 1, menu.getMenus(), menu.getNpcName(), menu.getNpcChat());
            } else if (menu.getAction() != null) {
                menu.perform();
            } else {
                switch (menu.getId()) {

                }
            }
            return;
        }
    }
}
