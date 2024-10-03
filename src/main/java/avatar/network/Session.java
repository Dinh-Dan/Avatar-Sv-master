package avatar.network;

import avatar.constants.Cmd;
import avatar.constants.NpcName;
import avatar.db.DbManager;
import avatar.handler.*;
import avatar.item.Item;
import avatar.item.PartManager;
import avatar.item.Part;
import avatar.lucky.DialLucky;
import avatar.lucky.DialLuckyManager;

import java.sql.Connection;
import java.text.MessageFormat;
import java.util.*;

import avatar.message.*;
import avatar.model.*;
import avatar.play.*;
import avatar.play.Map;
import avatar.service.*;
import org.json.simple.JSONValue;
import org.json.simple.JSONArray;

import java.sql.ResultSet;
import java.sql.PreparedStatement;
import java.sql.SQLException;

import java.io.IOException;

import avatar.server.Avatar;
import avatar.server.ServerManager;
import avatar.play.offline.AbsMapOffline;
import avatar.play.offline.MapOfflineManager;
import avatar.server.UserManager;
import avatar.server.Utils;
import lombok.Getter;

import java.net.InetSocketAddress;
import java.io.DataOutputStream;
import java.io.DataInputStream;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;

import avatar.play.Map;
import avatar.play.MapManager;
import avatar.play.NpcManager;
import static avatar.constants.NpcName.boss;
import static avatar.model.Boss.spawnBossesForMap;

public class Session implements ISession {

    private static final byte[] key = (System.currentTimeMillis() + "_kitakeyos").getBytes();
    public Socket sc;
    public DataInputStream dis;
    public DataOutputStream dos;
    public int id;
    public String ip;
    public User user;
    public GlobalHandler handler;
    private IMessageHandler messageHandler;
    public boolean connected;
    public boolean login;
    private byte curR;
    private byte curW;
    private final Sender sender;
    private Thread collectorThread;
    protected Thread sendThread;
    public final Object obj;
    protected String platform;
    protected String versionARM;
    private byte resourceType;
    @Getter
    private AvatarService avatarService;
    @Getter
    private FarmService farmService;
    @Getter
    private HomeService homeService;
    @Getter
    private ParkService parkService;
    @Getter
    private Service service;

    private UpgradeItemHandler upgradeHandler;


    private static final java.util.Map<Integer, Long> lastActionTimes = new HashMap<>();
    private static final long ACTION_COOLDOWN_MS = 100; //

    public Session(Socket sc, int id) throws IOException {
        this.obj = new Object();
        this.resourceType = 0;
        this.sc = sc;
        this.id = id;
        this.ip = ((InetSocketAddress) this.sc.getRemoteSocketAddress()).getAddress().toString().replace("/", "");
        this.dis = new DataInputStream(sc.getInputStream());
        this.dos = new DataOutputStream(sc.getOutputStream());
        this.setHandler(new MessageHandler(this));
        this.sendThread = new Thread(this.sender = new Sender());
        (this.collectorThread = new Thread(new MessageCollector())).start();
        this.avatarService = new AvatarService(this);
        this.farmService = new FarmService(this);
        this.homeService = new HomeService(this);
        this.parkService = new ParkService(this);
        this.service = new Service(this);
    }

    @Override
    public boolean isConnected() {
        return this.connected;
    }

    @Override
    public void setHandler(IMessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    @Override
    public void sendMessage(Message message) {
        this.sender.AddMessage(message);
    }

    protected synchronized void doSendMessage(Message m) {
        byte[] data = m.getData();
        try {
            if (this.connected) {
                byte b = this.writeKey(m.getCommand());
                this.dos.writeByte(b);
            } else {
                this.dos.writeByte(m.getCommand());
            }
            if (data != null) {
                int size = data.length;
                if (m.getCommand() == 90) {
                    this.dos.writeInt(size);
                    this.dos.write(data);
                } else {
                    if (this.connected) {
                        int byte1 = this.writeKey((byte) (size >> 8));
                        this.dos.writeByte(byte1);
                        int byte2 = this.writeKey((byte) (size & 0xFF));
                        this.dos.writeByte(byte2);
                    } else {
                        this.dos.writeShort(size);
                    }
                    if (this.connected) {
                        for (int i = 0; i < data.length; ++i) {
                            data[i] = this.writeKey(data[i]);
                        }
                    }
                    this.dos.write(data);
                }
            } else {
                this.dos.writeShort(0);
            }
            this.dos.flush();
            m.cleanup();
        } catch (IOException ex) {
        }
    }

    private byte readKey(byte b) {
        byte[] key = Session.key;
        byte curR = this.curR;
        this.curR = (byte) (curR + 1);
        byte i = (byte) ((key[curR] & 0xFF) ^ (b & 0xFF));
        if (this.curR >= Session.key.length) {
            this.curR %= (byte) Session.key.length;
        }
        return i;
    }

    private byte writeKey(byte b) {
        byte[] key = Session.key;
        byte curW = this.curW;
        this.curW = (byte) (curW + 1);
        byte i = (byte) ((key[curW] & 0xFF) ^ (b & 0xFF));
        if (this.curW >= Session.key.length) {
            this.curW %= (byte) Session.key.length;
        }
        return i;
    }

    @Override
    public void close() {
        try {
            if (this.user != null) {
                this.user.close();
                UserManager.getInstance().remove(user);
            }
            ServerManager.disconnect(this);
            this.cleanNetwork();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void cleanNetwork() {
        this.curR = 0;
        this.curW = 0;
        try {
            this.connected = false;
            this.login = false;
            this.dis.close();
        } catch (Exception ex) {
            try {
                this.dos.close();
            } catch (Exception ex2) {
                try {
                    this.sc.close();
                } catch (Exception ex3) {
                } finally {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                }
            } finally {
                try {
                    this.sc.close();
                } catch (Exception ex4) {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                } finally {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                }
            }
        } finally {
            try {
                this.dos.close();
            } catch (Exception ex5) {
                try {
                    this.sc.close();
                } catch (Exception ex6) {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                } finally {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                }
            } finally {
                try {
                    this.sc.close();
                } catch (Exception ex7) {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                } finally {
                    this.sendThread = null;
                    this.collectorThread = null;
                    System.gc();
                }
            }
        }
    }

    @Override
    public String toString() {
        if (this.user != null) {
            return this.user.toString();
        }
        return "Client " + this.id;
    }

    public boolean isResourceHD() {
        return this.resourceType == 1;
    }

    public String getResourcesPath() {
        return isResourceHD() ? ServerManager.resHDPath : ServerManager.resMediumPath;
    }

    public void handshakeMessage() throws IOException {
        Message ms = new Message(-27);
        DataOutputStream ds = ms.writer();
        ds.writeByte(Session.key.length);
        ds.writeByte(Session.key[0]);
        for (int i = 1; i < Session.key.length; ++i) {
            ds.writeByte(Session.key[i] ^ Session.key[i - 1]);
        }
        ds.flush();
        this.doSendMessage(ms);
        this.connected = true;
        this.sendThread.start();
    }

    public void getHandler(Message ms) throws IOException {
        byte index = ms.reader().readByte();
        System.out.println("getHandler: " + index);

        if (index == 8) {
            Zone zone = user.getZone();
            zone.leave(user);
        }
        ms = new Message(Cmd.GET_HANDLER);
        DataOutputStream ds2 = ms.writer();
        ds2.writeByte(index);
        ds2.flush();
        this.sendMessage(ms);
        switch (index) {
            case 3:
                setHandler(new CasinoMsgHandler(this));
                break;
            case 8: {
                setHandler(new AvatarMsgHandler(this));
                break;
            }
            case 9: {
                setHandler(new ParkMsgHandler(this));
                break;
            }
            case 10: {
                setHandler(new FarmMsgHandler(this));
                break;
            }
            case 11: {
                this.setHandler(new HomeMsgHandler(this));
                break;
            }
            default: {
                setHandler(new MessageHandler(this));
                break;
            }
        }
    }

    public void doGetImgIcon(Message ms) throws IOException {
        short imageID = ms.reader().readShort();
        String folder = this.getResourcesPath() + "object/";
        byte[] dat = Avatar.getFile(folder + imageID + ".png");
        if (dat == null) {
            return;
        }
        ms = new Message(Cmd.GET_IMG_ICON);
        DataOutputStream ds = ms.writer();
        ds.writeShort(imageID);
        ds.writeShort(dat.length);
        ds.write(dat);
        ds.flush();
        this.sendMessage(ms);

    }

    // -98 cmd
    public void requestImagePart(Message ms) throws IOException {
        short imageID = ms.reader().readShort();
        String folder = getResourcesPath() + "item/";
        byte[] dat = Avatar.getFile(folder + imageID + ".png");
        if (dat == null) {
            return;
        }
        ms = new Message(Cmd.REQUEST_IMAGE_PART);
        DataOutputStream ds = ms.writer();
        ds.writeShort(imageID);
        ds.writeShort(dat.length);
        ds.write(dat);
        ds.flush();
        this.sendMessage(ms);
    }

    public void doRequestExpicePet(Message ms) throws IOException {
        int userID = ms.reader().readInt();
        ms = new Message(-70);
        DataOutputStream ds = ms.writer();
        ds.writeInt(userID);
        ds.writeByte(0);
        ds.flush();
        this.sendMessage(ms);
    }

    public void clientInfo(Message ms) throws IOException {
        byte provider = ms.reader().readByte();
//        if(provider!=9) {
//            Utils.writeLog(this.user,"login infoFail : provider = "+provider);
//            this.user.session.getAvatarService().serverDialog("Kết nối thất bại. Xin kiểm tra kết nối wifi/3g");
//            this.user.session.close();
//        }
        int memory = ms.reader().readInt();
        String platform = ms.reader().readUTF();
        this.platform = platform;
        int rmsSize = ms.reader().readInt();
        int width = ms.reader().readInt();
        int height = ms.reader().readInt();
        boolean aaaaa = ms.reader().readBoolean();
        byte resource = ms.reader().readByte();
        this.resourceType = resource;
        String version = ms.reader().readUTF();
        if (ms.reader().available() > 0) {
            ms.reader().readUTF();
            ms.reader().readUTF();
            ms.reader().readUTF();
        }
    }

    public void agentInfo(Message ms) throws IOException {
        String agent = ms.reader().readUTF();
        System.out.println("agentInfo: " + agent);
    }


    public  void doRequestService(Message ms) throws IOException {
        byte id = ms.reader().readByte();
        //String msg = ms.reader().readUTF();
        switch (id) {
            case 0:{
                ms = new Message(Cmd.UPDATE_CONTAINER);
                DataOutputStream ds = ms.writer();
                String content = this.user.getUpgradeRequirements();
                ds.writeByte(0);
                ds.writeUTF(content);
                ds.flush();
                this.sendMessage(ms);
                break;
            }
            case 1:{
                ms = new Message(Cmd.UPDATE_CONTAINER);
                DataOutputStream ds = ms.writer();
                String content = this.user.upgradeChest();
                ds.writeByte(1);
                ds.writeUTF(content);
                ds.flush();
                this.sendMessage(ms);
                break;
            }
            case 6: {
                ms = new Message(-10);
                DataOutputStream ds = ms.writer();
                ds.writeUTF(String.format("Bạn đang đăng nhập vào thành phố %s. Dân số %d  người.", ServerManager.cityName, ServerManager.numClients));
                ds.flush();
                this.sendMessage(ms);
                break;
            }
            case 3: {
                ms = new Message(-10);
                DataOutputStream ds = ms.writer();
                ds.writeUTF("Chưa có game khác bạn ơiiiii !");
                ds.flush();
                this.sendMessage(ms);
                break;
            }
        }
    }

    public void doLogin(Message ms) throws IOException {
        if (this.login) {
            return;
        }
        String username = ms.reader().readUTF().trim();
        String password = ms.reader().readUTF().trim();
        String version = ms.reader().readUTF().trim();
        this.versionARM = version;
        User us = new User();
        us.setUsername(username);
        us.setPassword(password);
        us.setSession(this);
        boolean result = us.login();
        if (result) {
            this.login = true;
            this.user = us;
            enter();
        } else {
            this.login = false;
        }
    }

    private boolean isCharCreatedPopup;

    private void enter() throws IOException {
        if (user.loadData()) {
            DbManager.getInstance().executeUpdate("UPDATE `players` SET `is_online` = ?, `client_id` = ? WHERE `user_id` = ? LIMIT 1;", 1, this.id, user.getId());
            user.initAvatar();
            this.handler = new GlobalHandler(user);
            UserManager.getInstance().add(user);
            getAvatarService().onLoginSuccess();
            getAvatarService().serverDialog("Chào mừng bạn đã đến với Avatar Thanh Pho lo");
            getAvatarService().serverInfo("nạp x2 đến hết ngày 5/10 và update 2 shop PayToWin and ChayToWin ở ngoại ô mời các bạn khám phá, chúc các bạn online vui vẻ.");


            checkThuongNapLanDau();
            checkThuongNapSet();

            int diamondsPerThousand = 1; // Tặng 1 kim cương vũ trụ cho mỗi 1.000 VND đã nạp
            int tongNap = getTotalDeposited(user); // Tổng số tiền người chơi đã nạp
            int nhanthuongTongNap = getNhanThuongTongNap(); // Số tiền nạp đã nhận thưởng

            // Tính phần thưởng dựa trên tổng tiền nạp chưa nhận
            int rewardableAmount = (tongNap - nhanthuongTongNap) / 1000;

            if (rewardableAmount > 0) {
                // Tặng số kim cương tương ứng
                Item Kimcuong =  new Item(690,-1, rewardableAmount * diamondsPerThousand);
                this.user.addItemToChests(Kimcuong);
                this.user.getAvatarService().SendTabmsg("Bạn vừa donate nhận được " + rewardableAmount * diamondsPerThousand + " kim cương vũ trụ");
                // Cập nhật lại nhanthuongTongNap trong cơ sở dữ liệu
                int newNhanThuong = nhanthuongTongNap + (rewardableAmount * 1000);
                updateNhanThuongTongNap(newNhanThuong);
            }

            //NhanThuongEventluong();

            //NhanThuongEventXuBoss();

        } else {
            if (isCharCreatedPopup) {
                getAvatarService().serverDialog("Có lỗi xảy ra!");
                close();
                return;
            }
            isCharCreatedPopup = true;
            DbManager.getInstance().executeUpdate("INSERT INTO `players`(`user_id`, `level_main`, `gender`, `scores`) VALUES (?, ?, ?,?);", user.getId(), 1, 0,0);
            enter();
        }
    }

    public int getNhanThuongTongNap() {
        String sql = "SELECT nhanthuongTongNap FROM users WHERE id = ?";
        int nhanthuongTongNap = 0; // Mặc định là 0 nếu không có dữ liệu

        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {

            ps.setInt(1, this.user.getId());

            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    nhanthuongTongNap = rs.getInt("nhanthuongTongNap");
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return nhanthuongTongNap;
    }


    private void updateNhanThuongTongNap(int newNhanThuong) {
        String sql = "UPDATE users SET nhanthuongTongNap = ? WHERE id = ?";

        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, newNhanThuong);
            ps.setInt(2, this.user.getId());
            ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }


//paytowin
    public int getTotalDeposited(User us) {
        String sql = "SELECT tongNap FROM users WHERE id = ?";
        int totalDeposited = 0; // Mặc định tổng nạp là 0 nếu không tìm thấy

        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {

            ps.setInt(1, this.user.getId()); // Sử dụng ID người dùng để truy vấn

            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    totalDeposited = rs.getInt("tongNap"); // Lấy tổng tiền nạp từ cột 'tongNap'
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return totalDeposited; // Trả về tổng tiền nạp
    }


    private void NhanThuongEventXuBoss() throws IOException {

        int TopXuboss = this.user.getService().getUserRankXuBoss(user);

        if (checkXemNhanThuongXuboss(user)) {
            System.out.println("Người chơi " + user.getUsername() + " đã nhận thưởng.");
            return; // Nếu đã nhận thưởng, kết thúc hàm
        }

        if(TopXuboss > 5) {
            return;
        };


        List<Item> TOP5XUBOSS = new ArrayList<>();
        TOP5XUBOSS.add(new Item(2018,-1,1));

        List<Item> TOP3SET = new ArrayList<>();
        TOP3SET.add(new Item(4442,-1,1));
        TOP3SET.add(new Item(4443,-1,1));
        TOP3SET.add(new Item(4444,-1,1));




        if (TopXuboss == 1) {
            // Trao thưởng top 1
            if(user.chests.size() >= user.getChestSlot()-5){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 6 ô trống trong rương đồ để nhận thưởng top 1");
                return;
            }

            List<Item> phanThuongTop1boss = new ArrayList<>();
            phanThuongTop1boss.add(new  Item(2740,System.currentTimeMillis() + (86400000L * 7),1));//the vip
            phanThuongTop1boss.add(new  Item(6142,-1,1));//tóc superblue6

            Utils.writeLog(user,"Nhận Thưởng TOP 1 XU BOSS : ");
            for (Item item : TOP3SET) {
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            for (Item item : phanThuongTop1boss){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            for (Item item : TOP5XUBOSS){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            System.out.println("Username: " + user.getUsername() + ", rank" + TopXuboss);

        } else if (TopXuboss == 2 || TopXuboss == 3) {

            if(user.chests.size() >= user.getChestSlot()-4){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 5 ô trống trong rương đồ để nhận thưởng top lượng "+TopXuboss);
                return;
            }

            Utils.writeLog(user,"Nhận Thưởng TOP 2or3 xu boss :");
            Item theCaoCao =  new  Item(2740,System.currentTimeMillis() + (86400000L * 7),1);
            user.addItemToChests(theCaoCao);
            Utils.writeLog(user,theCaoCao.getPart().getName());

            System.out.println("Username: " + user.getUsername() + ", rank3" + TopXuboss);
            for (Item item : TOP5XUBOSS){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            for (Item item : TOP3SET) {
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }

        } else if (TopXuboss == 4 || TopXuboss == 5) {
            if(user.chests.size() >= user.getChestSlot()-1){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 2 ô trống trong rương đồ để nhận thưởng top lượng "+TopXuboss);
                return;
            }
            Utils.writeLog(user,"Nhận Thưởng TOP 4or5 Pháo Lượng :");
            for (Item item : TOP5XUBOSS){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
        }
        UpdateDaNhanThuongEventXuboss(user);
        user.getAvatarService().SendTabmsg("Bạn đã Nhận thưởng top "+TopXuboss);
    }

    private void UpdateDaNhanThuongEventXuboss(User us) {
        String sql = "UPDATE players SET XubossTop = TRUE WHERE user_id = ?";
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, us.getId()); // Sử dụng user_id để cập nhật
            ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private boolean checkXemNhanThuongXuboss(User us) {
        String sql = "SELECT XubossTop FROM players WHERE user_id = ?";
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, us.getId()); // Sử dụng user_id để truy vấn
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    return rs.getBoolean("XubossTop");
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false; // Mặc định trả về false nếu có lỗi
    }

    private void NhanThuongEventluong() throws IOException {

        int rankPhaoLuong = this.user.getService().getUserRankPhaoLuong(user);

        if (checkXemNhanThuongTopLuong(user)) {
            System.out.println("Người chơi " + user.getUsername() + " đã nhận thưởng.");
            return; // Nếu đã nhận thưởng, kết thúc hàm
        }

        if(rankPhaoLuong > 5) {
            return;
        };

        int slotChest = 5;

        List<Item> TOP5Luong = new ArrayList<>();
        TOP5Luong.add(new Item(4121,-1,1));
        TOP5Luong.add(new Item(4122,-1,1));
        TOP5Luong.add(new Item(4123,-1,1));


        if (rankPhaoLuong == 1) {
            // Trao thưởng top 1
            if(user.chests.size() >= user.getChestSlot()-4){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 5 ô trống trong rương đồ để nhận thưởng top 1");
                return;
            }

            List<Item> phanThuongTop1 = new ArrayList<>();
            phanThuongTop1.add(new  Item(2742,System.currentTimeMillis() + (86400000L * 7),1));//preium
            phanThuongTop1.add(new  Item(2741,System.currentTimeMillis() + (86400000L * 7),1));//cao cấp

            Utils.writeLog(user,"Nhận Thưởng TOP 1 Pháo Lượng : ");
            for (Item item : phanThuongTop1){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            for (Item item : TOP5Luong){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
            System.out.println("Username: " + user.getUsername() + ", rank" + rankPhaoLuong);

        } else if (rankPhaoLuong == 2 || rankPhaoLuong == 3) {

            if(user.chests.size() >= user.getChestSlot()-3){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 4 ô trống trong rương đồ để nhận thưởng top lượng "+rankPhaoLuong);
                return;
            }

            Utils.writeLog(user,"Nhận Thưởng TOP 2or3 Pháo Lượng :");
            Item theCaoCao =  new  Item(2741,System.currentTimeMillis() + (86400000L * 7),1);
            user.addItemToChests(theCaoCao);
            Utils.writeLog(user,theCaoCao.getPart().getName());

            System.out.println("Username: " + user.getUsername() + ", rank3" + rankPhaoLuong);
            for (Item item : TOP5Luong){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }


        } else if (rankPhaoLuong == 4 || rankPhaoLuong == 5) {
            if(user.chests.size() >= user.getChestSlot()-2){
                user.getAvatarService().SendTabmsg("Bạn phải có ít nhất 3 ô trống trong rương đồ để nhận thưởng top lượng "+rankPhaoLuong);
                return;
            }
            Utils.writeLog(user,"Nhận Thưởng TOP 4or5 Pháo Lượng :");
            for (Item item : TOP5Luong){
                this.user.addItemToChests(item);
                Utils.writeLog(user,item.getPart().getName());
            }
        }
        UpdateDaNhanThuongEventluong(user);
        user.getAvatarService().SendTabmsg("Bạn đã Nhận thưởng top "+rankPhaoLuong);
    }

    private void UpdateDaNhanThuongEventluong(User us) {
        String sql = "UPDATE players SET has_received_reward = TRUE WHERE user_id = ?";
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, us.getId()); // Sử dụng user_id để cập nhật
            ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private boolean checkXemNhanThuongTopLuong(User us) {
        String sql = "SELECT has_received_reward FROM players WHERE user_id = ?";
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(sql)) {
            ps.setInt(1, us.getId()); // Sử dụng user_id để truy vấn
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    return rs.getBoolean("has_received_reward");
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false; // Mặc định trả về false nếu có lỗi
    }

    ////////
    private void checkThuongNapLanDau(){
        String checkNap = "SELECT tongnap,ThuongNapLanDau FROM users WHERE id = ? LIMIT 1;";
        // Executing the query
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(checkNap);) {
            ps.setInt(1, user.getId());
            ResultSet rs = ps.executeQuery();
            while (rs.next()) {
                int tongnap = rs.getInt("tongnap");
                boolean napLanDau = rs.getBoolean("ThuongNapLanDau");
                if(!napLanDau && tongnap>=20000){
                    nhanThuongLanDau();
                }
            }
            rs.close();
        } catch (SQLException ex) {
            Logger.getLogger(Map.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    private void nhanThuongLanDau(){
        try {
            user.getAvatarService().SendTabmsg("Bạn vừa donate lần đầu trên 20k nhận được 5.000.000 xu và 10.000 lượng và 200 thẻ quay số miễn phí");

        } catch (IOException e) {
            throw new RuntimeException(e);
        }
        DbManager.getInstance().executeUpdate("UPDATE `users` SET `ThuongNapLanDau` = ? WHERE `id` = ? LIMIT 1;",
                1, user.getId());
        user.updateLuong(+10000);
        user.updateXu(+5000000);
        user.getAvatarService().updateMoney(0);
        Item item = new Item(593, -1, 200);
        user.addItemToChests(item);
    }

    ///////////

    private void checkThuongNapSet(){
        String checkNap = "SELECT tongnap,ThuongNapSet FROM users WHERE id = ? LIMIT 1;";
        // Executing the query
        try (Connection connection = DbManager.getInstance().getConnection();
             PreparedStatement ps = connection.prepareStatement(checkNap);) {
            ps.setInt(1, user.getId());
            ResultSet rs = ps.executeQuery();
            while (rs.next()) {
                int tongnap = rs.getInt("tongnap");
                boolean thuongNapSet = rs.getBoolean("ThuongNapSet");
                if(!thuongNapSet && tongnap>=100000){
                    nhanThuongNapSet();
                }
            }
            rs.close();
        } catch (SQLException ex) {
            Logger.getLogger(Map.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    private void nhanThuongNapSet(){
        try {

            user.getAvatarService().SendTabmsg("Bạn nhận được set siêu sao âm nhạc");

        } catch (IOException e) {
            throw new RuntimeException(e);
        }
        if(user.chests.size() >= user.getChestSlot()-4){
            user.getAvatarService().serverDialog("Bạn phải có ít nhất 5 ô trống trong rương đồ");
            return;
        }

        DbManager.getInstance().executeUpdate("UPDATE `users` SET `ThuongNapSet` = ? WHERE `id` = ? LIMIT 1;",
                1, user.getId());

        if(user.getGender() == 1)
        {
            Item toc = new Item(5750);//tóc nam
            toc.setExpired(-1);
            user.addItemToChests(toc);

            Item ao = new Item(5751);
            ao.setExpired(-1);
            user.addItemToChests(ao);

            Item quan = new Item(5752);
            quan.setExpired(-1);
            user.addItemToChests(quan);

            Item bongtai = new Item(5753);
            bongtai.setExpired(-1);
            user.addItemToChests(bongtai);

        }else {
            Item tocnu = new Item(5755);// nu
            tocnu.setExpired(-1);
            user.addItemToChests(tocnu);

            Item aonu = new Item(5756);// nu
            aonu.setExpired(-1);
            user.addItemToChests(aonu);

            Item quanNu = new Item(5757);// nu
            quanNu.setExpired(-1);
            user.addItemToChests(quanNu);

            Item bongTaiNu = new Item(5759);// nu
            bongTaiNu.setExpired(-1);
            user.addItemToChests(bongTaiNu);

        }

        Item diaBayAmNhac = new Item(5760);
        diaBayAmNhac.setExpired(-1);
        user.addItemToChests(diaBayAmNhac);
    }

    public boolean isNewVersion() {
        return true;
    }

    public void regMessage(Message ms) throws IOException {
        String username = ms.reader().readUTF().trim();
        String password = ms.reader().readUTF().trim();
    }

    public void createCharacter(Message ms) throws IOException {
        byte gender = ms.reader().readByte();//1 nam 2 nu
        byte numItem = ms.reader().readByte();
        ArrayList<Item> items = new ArrayList<>();
        short[] boyItems = { 89, 88, 0, 4, 14};
        short[] girlItems = { 89, 88, 0, 4, 49 };
        short[] selectedItems = (gender == 1) ? boyItems : girlItems;

        for (short itemID : selectedItems) {
            items.add(new Item(itemID, -1, 1));
        }
        boolean isError = false;
        if (gender != 1 && gender != 2) {
            isError = true;
        }
        isError = !CreateChar.getInstance().check(gender, items);
        if (isError) {
            ms = new Message(-35);
            DataOutputStream ds = ms.writer();
            ds.writeBoolean(false);
            ds.flush();
            this.sendMessage(ms);
            return;
        }
        user.setGender(gender);
        user.setWearing(items);
        ms = new Message(-35);
        DataOutputStream ds = ms.writer();
        ds.writeBoolean(true);
        ds.flush();
        this.sendMessage(ms);
    }

    public void doiKhuVuc(Message ms) throws IOException {
        if (this.messageHandler instanceof FarmMsgHandler) {
            return;
        }
        byte numKhuVuc = 10;
        byte mapid = ms.reader().readByte();
        Map m = MapManager.getInstance().find(mapid);
        ms = new Message(60);
        DataOutputStream ds = ms.writer();
        ds.writeByte(numKhuVuc);
        for (Zone zone : m.getZones()) {
            if (zone.getPlayers().size() >= 9) {
                ds.writeByte(0);
            } else if (zone.getPlayers().size() >= 4) {
                ds.writeByte(1);
            } else {
                ds.writeByte(2);
            }
        }
        ds.flush();
        this.sendMessage(ms);

        ds.close();
    }

    public void doJoinHouse4(Message ms) throws IOException {
        System.out.println("-104:  " + ms.reader().readInt());
    }

    public void buyItemShop(Message ms) {
        try {
            if(this.user.checkFullSlotChest()) {
                return;
            }

            short partID = ms.reader().readShort();
            byte type = ms.reader().readByte();
            if (type < 1 || type > 2) {
                this.user.getService().serverDialog("Có lỗi xảy ra, vui lòng liên hệ admin. Mã lỗi: buyItemShopWrongType");
                return;
            }
            Part part = PartManager.getInstance().findPartByID(partID);

            if (((part.getGender() == 2 || part.getGender() == 1) && (user.getGender() != part.getGender())))
            {
                user.getAvatarService().serverDialog("gioi tinh khong phu hop");
                return;
            }
            if (part.getName() == null){
                user.getAvatarService().serverDialog("ITEM lỗi mua item khác tạm đi bro");
                return;
            }
            if (part != null) {
                int priceXu = part.getCoin();
                int priceLuong = part.getGold();
                int price = 0;
                if ((priceXu == -1 && priceLuong == -1) || (type == 1 && priceXu == -1)
                        || (type == 2 && priceLuong == -1)) {
                    return;
                }
                if (priceXu > 0) {
                    price = priceXu;
                    if (user.getXu() < price) {
                        this.user.getService().serverMessage("Bạn không đủ xu!");
                        return;
                    }
                    this.user.updateXu(-price);
                } else {
                    price = priceLuong;
                    if (user.getLuong() < price) {
                        this.user.getService().serverMessage("Bạn không đủ lượng!");
                        return;
                    }
                    this.user.updateLuong(-price);
                }
                long expired = System.currentTimeMillis() + ((long) part.getExpiredDay() * 86400000L);
                if (part.getExpiredDay() == 0) {
                    expired = -1;
                }
                Item item = Item.builder()
                        .id(part.getId())
                        .expired(expired)
                        .build();
                System.out.println("expired: " + expired);

                int zOrder = part.getZOrder();
                Item w = user.findItemWearingByZOrder(zOrder);
                if (w != null) {
                    user.removeItemFromWearing(w);
                    user.addItemToChests(w);
                }

                user.addItemToWearing(item);
                user.removeItemFromChests(item);
                user.sortWearing();
                user.getMapService().usingPart(id, (short) item.getId());

                ms = new Message(-24);
                DataOutputStream ds = ms.writer();
                ds.writeShort(partID);
                if (partID != -1) {
                    ds.writeInt(price);
                    ds.writeByte(1);
                }
                ds.writeUTF("Bạn đã mua vật phẩm thành công.");
                ds.writeInt(Math.toIntExact(user.getXu()));
                ds.writeInt(user.getLuong());
                ds.writeInt(user.getLuongKhoa());
                ds.flush();
                this.sendMessage(ms);
            } else {
                this.avatarService.serverMessage("Vật phẩm không tồn tại !!!");
            }
        } catch (Exception e) {
            System.out.println("[ERROR-DB]" + e.getMessage());
        }
    }

    public void doJoinOfflineMap(Message ms) throws IOException {
        byte map = ms.reader().readByte();
        AbsMapOffline mapOffline = MapOfflineManager.getInstance().find(map);
        List<Npc> npcs = new ArrayList<>();
        if (mapOffline != null) {
            npcs = mapOffline.getNpcs();
        } else {
            System.out.println("Map offline join: " + map);
        }
        ms = new Message(Cmd.JOIN_OFFLINE_MAP);
        DataOutputStream ds = ms.writer();
        ds.writeByte(map);
        ds.writeByte(npcs.size());
        for (Npc npc : npcs) {
            ds.writeInt(npc.getId());
            ds.writeUTF(npc.getUsername());
            List<Item> wearing = npc.getWearing();
            ds.writeByte(wearing.size());
            for (Item item : wearing) {
                ds.writeShort(item.getId());
            }
            ds.writeShort(npc.getX());
            ds.writeShort(npc.getY());
            ds.writeByte(npc.getStar());
            ds.writeByte(0);
            ds.writeShort(npc.getIdImg());
            List<String> chats = npc.getTextChats();
            ds.writeByte(chats.size());
            for (String text : chats) {
                ds.writeUTF(text);
            }
        }
        ds.writeShort(0);
        ds.flush();
        this.sendMessage(ms);
    }

    public void doRequestCityMap(Message ms) throws IOException {
        if (ms.reader().available() > 0) {
            byte idMini = ms.reader().readByte();
            System.out.println("RequestCityMap: " + idMini);
        }
        ms = new Message(-63);
        DataOutputStream ds = ms.writer();
        ds.writeByte(-1);
        ds.flush();
        this.sendMessage(ms);
        user.getAvatarService().openMenuOption(5, 0, "Đảo Hawaii", "Ai Cập", "Vương Quốc Bóng Đêm", "Biển citylo");
    }

    public void doCommunicate(Message ms) throws IOException {
        int userId = ms.reader().readInt();
        if (userId >= 2000000000) {
            NpcHandler.handlerCommunicate(userId, this.user);
            return;
        } else {
            System.out.println("userId = " + userId);
            if (userId == 0) {
                // hiện thị menu chức năng
                List<Menu> menus = new ArrayList<>(List.of(
                        Menu.builder().name("Auto Câu Cá").menus(
                                        List.of(
                                                Menu.builder().name("Kích hoạt Auto Câu Cá").action(() -> {
                                                    String checkNap = "SELECT ThuongNapLanDau FROM users WHERE id = ? LIMIT 1;";
                                                    try (Connection connection = DbManager.getInstance().getConnection();
                                                         PreparedStatement ps = connection.prepareStatement(checkNap);) {
                                                        ps.setInt(1, user.getId());
                                                        ResultSet rs = ps.executeQuery();
                                                        while (rs.next()) {
                                                            boolean napLanDau = rs.getBoolean("ThuongNapLanDau");
                                                            if(napLanDau){
                                                                this.user.getAvatarService().serverDialog("Bạn Đã Kích Hoạt Auto Câu Cá Thành Công");
                                                                this.user.setAutoFish(true);
                                                            }
                                                            else {
                                                                this.user.getAvatarService().serverDialog("Treo câu thì donate lần đầu nha : v");
                                                                this.user.setAutoFish(false);
                                                            }
                                                        }
                                                        rs.close();
                                                    } catch (SQLException ex) {
                                                        Logger.getLogger(Map.class.getName()).log(Level.SEVERE, null, ex);
                                                    }
                                                }).build(),
                                                Menu.builder().name("Tắt Auto Câu Cá").action(() -> {
                                                    this.user.getAvatarService().serverDialog("Bạn Đã Tắt Auto Câu Cá");
                                                    this.user.setAutoFish(false);
                                                }).build()
                                        ))
                                .build(),
                        Menu.builder().name("Mã quà tặng(gift code)").action(() -> {
                            user.getAvatarService().sendTextBoxPopup(user.getId(), 20, "Item code", 1);
                        }).build(),
                        Menu.builder().name("Mã Giới Thiệu").build(),
                        Menu.builder().name("Diễn Đàn").build()
                ));
                if (user.getId() == 7) {
                    menus.add(0, Menu.builder().name("Admin")
                            .menus(List.of(
                                    Menu.builder().name("Thêm item").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 7, "Item code", 1);
                                    }).build(),
                                    Menu.builder().name("Fix Lỗi Rương").action(() -> {
                                        try {
                                            System.out.println("fix Item id : " + user.getUsername());
                                            List<Item> items = user.getChests();
                                            int itemIndex = items.size() - 1;
                                            System.out.println("index: " + itemIndex);
                                            Item item = items.get(itemIndex);
                                            System.out.println("fix Item id : " + user.getUsername());
                                            user.removeItem(item.getId(), 1);
                                            user.getAvatarService().serverDialog("ok");
                                        } catch (NumberFormatException e) {
                                            user.getAvatarService().serverDialog("error");
                                        }
                                    }).build(),
                                    Menu.builder().name("Chat tổng").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 8, "thong bao", 1);
                                    }).build(),
                                    Menu.builder().name("Thời Tiết").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 9, "thoi tiet", 1);
                                    }).build(),
                                    Menu.builder().name("bao tri").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 10, "bao tri", 1);
                                    }).build(),
                                    Menu.builder().name("infor").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 11, "infor", 1);
                                    }).build(),
                                    Menu.builder().name("thread?").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 12, "thread", 1);
                                    }).build(),
                                    Menu.builder().name("pem").action(() -> {
                                        if(user.getId() == 7||user.getId() == 97){
                                            user.getZone().getPlayers().forEach(u -> {
                                                EffectService.createEffect()
                                                        .session(u.session)
                                                        .id((byte) 23)
                                                        .style((byte) 0)
                                                        .loopLimit((byte) 5)
                                                        .loop((short) 1)
                                                        .loopType((byte) 1)
                                                        .radius((short) 5)
                                                        .idPlayer(user.getId())
                                                        .send();
                                            });
                                        }else{
                                            user.getAvatarService().serverDialog("ad mới bật được b ơi");
                                        }
                                    }).build(),
                                    Menu.builder().name("Tim Rơi").action(() -> {
                                        if(user.getId() == 7){
                                            user.getZone().getPlayers().forEach(u -> {
                                                EffectService.createEffect()
                                                        .session(u.session)
                                                        .id((byte) 56)
                                                        .style((byte) 0)
                                                        .loopLimit((byte) 5)
                                                        .loop((short) 100)
                                                        .loopType((byte) 1)
                                                        .radius((short) 250)
                                                        .idPlayer(user.getId())
                                                        .send();
                                            });
                                        }else{
                                            user.getAvatarService().serverDialog("ad mới bật được b ơi");
                                        }
                                    }).build(),
                                    Menu.builder().name("Menu sentb bao tri").action(() -> {
                                        user.getAvatarService().sendTextBoxPopup(user.getId(), 98, "bao tri sau 2p", 1);
                                    }).build(),
                                    Menu.builder().name("EFFECT").action(() -> {
                                        if(user.getId() == 7){

                                            user.getAvatarService().sendTextBoxPopup(user.getId(), 99, "ideffect", 1);

                                        }else{
                                            user.getAvatarService().serverDialog("ad mới bật được b ơi");
                                        }
                                    }).build(),
                                    Menu.builder().name("Khoá nick").build(),
                                    Menu.builder().name("Tặng item").build()
                            ))
                            .build());
                }
                user.setMenus(menus);
                user.getAvatarService().openUIMenu(1, 0, menus, null, null);

            }
//            handleSelectFunction(this.user);

        }
    }



    public void handleBossShop(Message ms) throws IOException {
        int idBoss = ms.reader().readInt();
        byte type = ms.reader().readByte();
        short indexItem = ms.reader().readShort();
        if (idBoss == Npc.ID_ADD + NpcName.THO_KIM_HOAN && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do upgrade item boss shop {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem upgradeItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (upgradeItem != null) {
                Item item = user.findItemInChests(upgradeItem.getItemNeed());
                if (item == null) {
                    Part part = PartManager.getInstance().findPartById(upgradeItem.getItemNeed());
                    getService().serverDialog(MessageFormat.format("Bạn cần có {0} để nâng cấp món đồ này", part.getName()));
                    return;
                }
                if (type == BossShopHandler.SELECT_XU) {
                    if (upgradeItem.isOnlyLuong()) {
                        getService().serverDialog("Vật phẩm này chỉ có thể nâng cấp bằng lượng");
                        return;
                    }
                    if (user.getXu() < upgradeItem.getXu()) {
                        getService().serverDialog(MessageFormat.format("Bạn cần có {0} xu để nâng cấp món đồ này", upgradeItem.getXu()));
                        return;
                    }
                    user.updateXu(-upgradeItem.getXu());
                    doFinalUpgrade(upgradeItem, item);
                    return;
                } else if (type == BossShopHandler.SELECT_LUONG) {
                    if (user.getLuong() < upgradeItem.getLuong()) {
                        getService().serverDialog(MessageFormat.format("Bạn cần có {0} lượng để nâng cấp món đồ này", upgradeItem.getLuong()));
                        return;
                    }
                    user.updateLuong(-upgradeItem.getLuong());
                    doFinalUpgrade(upgradeItem, item);
                    return;
                }
            }
        }
        if (idBoss == Npc.ID_ADD + NpcName.Chay_To_Win && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do Event item boss shop Chay_To_Win {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem EventItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (EventItem != null) {
                doFinalEventShop(EventItem,NpcName.Chay_To_Win);
                return;
            }
        }

        if (idBoss == Npc.ID_ADD + NpcName.Pay_To_Win && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do Event item boss shop Pay_To_Win {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem EventItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (EventItem != null) {
                doFinalEventShop(EventItem,NpcName.Pay_To_Win);
                return;
            }
        }

        if (idBoss == Npc.ID_ADD + NpcName.bunma && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do Event item boss shop {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem EventItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (EventItem != null) {
                doFinalEventShop(EventItem,NpcName.bunma);
                return;
            }
        }
        if (idBoss == Npc.ID_ADD + NpcName.Vegeta && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do Event item boss shop vegenta {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem EventItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (EventItem != null) {
                doFinalEventShop(EventItem,NpcName.Vegeta);
                return;
            }
        }
        if (idBoss == Npc.ID_ADD + NpcName.Shop_Dac_Biet && user.getBossShopItems() != null) {
            System.out.println(MessageFormat.format("do Event item boss shop ShopDacBiet {0}, {1}, {2},"
                    , idBoss, type, indexItem));
            UpgradeItem EventItem = (UpgradeItem) user.getBossShopItems().get(indexItem);
            if (EventItem != null) {
                doFinalEventShop(EventItem,NpcName.Shop_Dac_Biet);
                return;
            }
        }
    }


    private void doFinalEventShopThuong(Item item,int npcId) {
        // pt thành dng item ko cần build qua updare
        Zone z = user.getZone();
        if (z != null) {
            User u = z.find(npcId+Npc.ID_ADD);
            if (u == null) {
                return;
            }
        } else {
            return;
        }
        switch (npcId) {
            case NpcName.Shop_Dac_Biet:
                if(user.getLuong()> item.getPart().getGold()){
                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    item.setExpired(-1);
                    user.addItemToChests(item);
                    user.updateLuong(-item.getPart().getGold());
                    getAvatarService().requestYourInfo(user);
                    getAvatarService().updateMoney(0);
                    getService().serverDialog("Chúc mừng bạn đã đổi thành công");
                } else {
                    getService().serverDialog("Bạn chưa đủ điều kiện để đổi");
                }
                break;
        }
    }


    private void doFinalEventShop(UpgradeItem Eventitem,int npcId) {
        Zone z = user.getZone();
        if (z != null) {
            User u = z.find(npcId+Npc.ID_ADD);
            if (u == null) {
                return;
            }
        } else {
            return;
        }
        switch (npcId) {
            case NpcName.Chay_To_Win:
                if(user.getXu()> Eventitem.getItem().getPart().getCoin()){
                    if (!isGenderCompatible(Eventitem.getItem(),this.user)){
                        getAvatarService().serverDialog("Giới tính không phù hợp !");
                        return;
                    }

                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    Eventitem.getItem().setExpired(-1);
                    user.updateXu(-Eventitem.getXu());
                    getAvatarService().updateMoney(0);
                    user.addItemToChests(Eventitem.getItem());
                    getService().serverDialog("Chúc mừng bạn đã đổi thành công");
                } else {
                    getService().serverDialog("Bạn chưa đủ Xu để đổi");
                }
                break;
            case NpcName.Pay_To_Win:
                Item huyhieu = this.user.findItemInChests(Eventitem.getItemNeed());
                if(huyhieu!=null&& huyhieu.getQuantity() >= Eventitem.getScores()){
                    Eventitem.getItem().setExpired(-1);
                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    if(Eventitem.getItem().getId() == 4345) {
                        user.removeItem(huyhieu.getId(),Eventitem.getScores());
                        Item quanpika = new Item(4346);
                        quanpika.setExpired(-1);
                        user.addItemToChests(quanpika);

                        Item aopika = new Item(4347);
                        aopika.setExpired(-1);
                        user.addItemToChests(aopika);

                        getAvatarService().requestYourInfo(user);

                        getService().serverDialog(String.format("Chúc mừng bạn đã đổi thành công %s",Eventitem.getItem().getPart().getName()));
                        //user.addItemToChests(Eventitem.getItem());
                        //return;
                    }
                    user.removeItem(huyhieu.getId(),Eventitem.getScores());
                    getAvatarService().requestYourInfo(user);
                    getService().serverDialog(String.format("Chúc mừng bạn đã đổi thành công %s",Eventitem.getItem().getPart().getName()));
                    if(Eventitem.getItem().getId() == 5408 || Eventitem.getItem().getId() == 5324 || Eventitem.getItem().getId() == 5880)
                    {
                        Item hopqua = new Item(Eventitem.getItem().getId(),-1,1);
                        //hopqua.setExpired(System.currentTimeMillis() + (86400000L * time));
                        if(user.findItemInChests(Eventitem.getItem().getId()) !=null){
                            int quantity = user.findItemInChests(Eventitem.getItem().getId()).getQuantity();
                            user.findItemInChests(Eventitem.getItem().getId()).setQuantity(quantity+1);
                        }else {
                            user.addItemToChests(hopqua);
                        }
                        return;
                    }

                    user.addItemToChests(Eventitem.getItem());
                }
                else
                {
                    getService().serverDialog(String.format("Bạn không đủ Kim cương vũ trụ để đổi"));
                }
                break;
            case NpcName.bunma:
                if(user.getScores()> Eventitem.getScores()){
                    Eventitem.getItem().setExpired(-1);
                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    user.addItemToChests(Eventitem.getItem());
                    user.setStylish((byte) (user.getStylish() - 1));
                    user.updateScores(-Eventitem.getScores());
                    getAvatarService().requestYourInfo(user);
                    getService().serverDialog("Chúc mừng bạn đã đổi thành công");
                } else {
                    getService().serverDialog("Bạn chưa đủ điểm để đổi");
                }
                break;
            case NpcName.Vegeta:
                Item TheVip = user.findItemInChests(Eventitem.getItemNeed());
                if(TheVip!=null){
                    Eventitem.getItem().setExpired(-1);
                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    user.addItemToChests(Eventitem.getItem());
                    user.setStylish((byte) (user.getStylish() - 1));
                    user.removeItemFromChests(TheVip);
                    getAvatarService().requestYourInfo(user);
                    getService().serverDialog(String.format("Chúc mừng bạn đã đổi thành công %s",Eventitem.getItem().getPart().getName()));
                } else {
                    getService().serverDialog(String.format("Bạn không có %s để đổi",Eventitem.getItemNeed()));
                }
                break;
            case NpcName.Shop_Dac_Biet:
                if(user.getLuong()> Eventitem.getItem().getPart().getGold()){
                    if(user.getChestSlot() <= user.chests.size())
                    {
                        getAvatarService().serverDialog("Rương đồ đã đầy");
                        return;
                    }
                    Eventitem.getItem().setExpired(-1);
                    user.addItemToChests(Eventitem.getItem());
                    user.updateLuong(-Eventitem.getItem().getPart().getGold());
                    getAvatarService().updateMoney(0);
                    getService().serverDialog("Chúc mừng bạn đã đổi thành công");
                } else {
                    getService().serverDialog("Bạn chưa đủ điều kiện để đổi");
                }
                break;
        }
    }
    private boolean isGenderCompatible(Item item, User user) {
        int itemGender = item.getPart().getGender(); // Giới tính của item (0 = cả hai giới, 1 = nam, 2 = nữ)
        int userGender = user.getGender(); // Giới tính của user (1 = nam, 2 = nữ)

        // Nếu itemGender là 0, thì cả hai giới đều dùng được
        if (itemGender == 0) {
            return true;
        }

        // Nếu không, kiểm tra xem giới tính của item có khớp với giới tính của user không
        return itemGender == userGender;
    }
    private void doFinalUpgrade(UpgradeItem item, Item itemOld) {

        long currentTime = System.currentTimeMillis();
        long lastActionTime = lastActionTimes.getOrDefault(this.user.getId(), 0L);
        if (currentTime - lastActionTime < ACTION_COOLDOWN_MS) {
            this.user.getAvatarService().serverDialog("Từ từ thôi bạn!");
            return;
        }
        lastActionTimes.put(this.user.getId(), currentTime);
        if(itemOld.getExpired()!=-1){
            user.getAvatarService().serverDialog("Bạn cần có vật phẩm "+itemOld.getPart().getName()+ " vĩnh viễn");
            return;
        }
        int ratio = item.getRatio();
        boolean isUpgradeSuccess = false;
        if (ratio > 0) {
            isUpgradeSuccess = Utils.nextInt(0, 100) < ratio;
        } else {
            ratio = Math.abs(ratio);
            int correctNumber = Utils.nextInt(0, ratio);
            isUpgradeSuccess = correctNumber == Utils.nextInt(0, ratio);
        }
        if (isUpgradeSuccess) {
            user.removeItemFromChests(itemOld);
            item.getItem().setExpired(-1);
            user.addItemToChests(item.getItem());
            getAvatarService().updateMoney(0);
            user.setStylish((byte) (user.getStylish() - 1));
            getAvatarService().requestYourInfo(user);
            List<User> players = this.user.getZone().getPlayers();
            for (User player : players) {
                EffectService.createEffect()
                        .session(player.session)
                        .id((byte)16)
                        .style((byte) 0)
                        .loopLimit((byte) 5)
                        .loop((short) 1)
                        .loopType((byte) 1)
                        .radius((short) 1)
                        .idPlayer(NpcName.THO_KIM_HOAN+Npc.ID_ADD)
                        .send();
            };


            getService().serverDialog("Chúc mừng bạn đã ghép đồ thành công");

            Zone z = user.getZone();
            if (z != null) {
                Npc npc = NpcManager.getInstance().find(z.getMap().getId(), z.getId(), NpcName.THO_KIM_HOAN + Npc.ID_ADD);
                if (npc == null) {
                    return;
                }
                npc.setTextChats(List.of(MessageFormat.format("Chúc mừng bạn {0} đã nâng cấp vật phẩm {1} thành công", user.getUsername(), item.getItem().getPart().getName())));
            } else {
                return;
            }
        } else {
            getAvatarService().updateMoney(0);
            getService().serverDialog("Ghép đồ thất bại. Chúc bạn may mắn lần sau");
        }
    }

    public void doDialLucky(Message ms) throws IOException {
        short partId = ms.reader().readShort();
        short degree = ms.reader().readShort();
        DialLucky dl = user.getDialLucky();
        if (dl != null) {
            if (dl.getType() == DialLuckyManager.MIEN_PHI) {
                Item itm = user.findItemInChests(593);
                if (itm == null || itm.getQuantity() <= 0) {
                    return;
                }
                user.removeItem(593, 1);
            }
            if (dl.getType() == DialLuckyManager.XU) {
                if (user.getXu() < 15000) {
                    return;
                }
                user.updateXu(-25000);
            }
            if (dl.getType() == DialLuckyManager.LUONG) {
                if (user.getLuong() < 5) {
                    return;
                }
                user.updateLuong(-5);
            }
            getAvatarService().updateMoney(0);
            dl.doDial(user, partId, degree);
        }
    }

    public void requestTileMap(Message ms) throws IOException {
        byte idTileImg = ms.reader().readByte();
        System.out.println("map = " + idTileImg);
        byte[] dat = Avatar.getFile(getResourcesPath() + "tilemap/" + idTileImg + ".png");
        if (dat == null) {
            return;
        }
        ms = new Message(Cmd.REQUEST_TILE_MAP);
        DataOutputStream ds = ms.writer();
        ds.writeByte(idTileImg);
        ds.write(dat);
        ds.flush();
        this.sendMessage(ms);
    }

    public void doParkBuyItem(Message ms) throws IOException {
        short id = ms.reader().readShort();
        Food food = FoodManager.getInstance().findFoodByFoodID(id);
        if (food != null) {
            int shop = food.getShop();
            int price = food.getPrice();
            if (price > user.xu) {
                this.user.getService().serverDialog("Bạn không đủ xu!");
                return;
            }
            String name = food.getName();
            this.user.updateXu(-price);
            if (shop == 4) {
                int health = 100 - this.user.getHunger() + food.getPercentHelth();
                health = ((health > 100) ? 100 : health);
                this.user.updateHunger(100 - health);
                this.user.getAvatarService()
                        .serverDialog(String.format("Bạn đã ăn một %s sức khoẻ bạn hiện tại là %d", name, health));
            } else if (shop == 5) {
                this.user.getService().serverDialog("Bạn đã cho thú nuôi ăn thành công");
            }
        }
    }

    public void requestFriendList(Message ms) throws IOException {
        ms = new Message(-81);
        DataOutputStream ds = ms.writer();
        ds.flush();
    }

    public void joinHouse(Message ms) throws IOException {
        int userId = ms.reader().readInt();
        Vector<HouseItem> hItems = new Vector<>();

        try (Connection connection = DbManager.getInstance().getConnection();) {
            String GET_HOUSE_DATA = "SELECT * FROM `house_buy` WHERE `user_id` = ? LIMIT 1";
            PreparedStatement ps = connection.prepareStatement(GET_HOUSE_DATA);
            ps.setInt(1, userId);
            ResultSet res = ps.executeQuery();

            if (res.next()) {
                JSONArray ja_map = (JSONArray) JSONValue.parse(res.getString("map_data"));
                byte[] map_data = new byte[ja_map.size()];
                for (int i = 0; i < ja_map.size(); ++i) {
                    map_data[i] = ((Long) ja_map.get(i)).byteValue();
                }
                ps.close();
                res.close();

                String GET_ITEMS_IN_CHEST = "SELECT * FROM `house_player_item` WHERE `user_id` = ?";
                ps = connection.prepareStatement(GET_ITEMS_IN_CHEST);
                ps.setInt(1, userId);
                res = ps.executeQuery();

                if (res != null) {
                    while (res.next()) {
                        HouseItem hItem = new HouseItem();
                        hItem.itemId = res.getShort("house_item_id");
                        hItem.x = res.getByte("x");
                        hItem.y = res.getByte("y");
                        hItem.rotate = res.getByte("rotate");
                        hItems.add(hItem);
                    }
                }
                ps.close();
                res.close();


                this.user.getZone().leave(user);
                ms = new Message(-65);
                DataOutputStream ds = ms.writer();
                ds.writeByte(3);
                ds.writeInt(this.user.getId());
                ds.writeShort(map_data.length);
                for (int j = 0; j < map_data.length; ++j) {
                    ds.write(map_data[j]);
                }
                ds.writeByte(28);
                ds.writeShort(hItems.size());
                for (HouseItem hItem2 : hItems) {
                    ds.writeShort(hItem2.itemId);
                    ds.writeByte(hItem2.x);
                    ds.writeByte(hItem2.y);
                    ds.writeByte(hItem2.rotate);
                }
                ds.flush();
                this.sendMessage(ms);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void BuyHouse() throws IOException {

        int userId =  this.user.session.user.getId();
        try (Connection connection = DbManager.getInstance().getConnection();) {
            String GET_HOUSE_DATA = "SELECT * FROM `house_buy` WHERE `user_id` = ? LIMIT 1";
            PreparedStatement ps = connection.prepareStatement(GET_HOUSE_DATA);
            ps.setInt(1, userId);
            ResultSet res = ps.executeQuery();

            if (res.next()) {
                this.user.session.avatarService.serverDialog("Bạn đã mua nhà rồi !");
                return;
            }
            if(this.user.session.user.getXu() < 1000000)
            {
                this.user.session.avatarService.serverDialog("Bạn không đủ tiền để mua nhà !");
                return;
            }

            this.user.updateXu(-1000000);
            this.user.getAvatarService().updateMoney(0);
            // Nếu chưa mua, tiến hành chèn dữ liệu nhà mới
            String INSERT_HOUSE_DATA = "INSERT INTO `house_buy` (user_id, type, map_data, date_expired) VALUES (?, ?, ?, ?)";
            try (PreparedStatement insertPs = connection.prepareStatement(INSERT_HOUSE_DATA)) {
                insertPs.setInt(1, userId);
                insertPs.setInt(2, 3); // `type`: thay đổi theo logic của bạn
                insertPs.setString(3, "[39,39,39,39,34,35,35,35,35,35,35,34,35,35,35,34,35,35,35,35,35,35,34,39,39,39,39,39,39,39,39,39,38,25,25,25,25,25,25,36,27,27,27,36,25,25,25,25,25,25,37,39,39,39,39,39,39,39,39,39,38,26,26,26,26,26,26,36,28,28,28,36,26,26,26,26,26,26,37,39,39,39,39,39,39,39,39,39,38,14,14,14,14,14,14,36,3,3,3,36,14,14,14,14,14,14,37,39,39,39,39,39,39,39,39,39,38,14,14,14,14,14,14,25,3,3,3,25,14,14,14,14,14,14,37,39,39,39,39,39,39,39,39,39,38,14,14,14,14,14,14,26,3,3,3,26,14,14,14,14,14,14,37,39,39,39,39,39,39,39,39,39,38,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,37,39,39,39,39,39,39,39,39,39,25,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,37,39,39,39,39,39,34,35,35,35,25,35,35,35,35,35,35,38,14,14,14,14,14,14,14,14,14,14,25,39,39,39,39,39,38,27,27,27,36,23,23,23,23,23,23,36,0,0,35,35,35,35,35,35,35,35,25,35,35,35,35,34,38,28,28,28,36,24,24,24,24,24,24,36,0,0,23,23,23,23,23,23,23,23,36,23,23,23,23,36,38,17,17,17,25,9,9,18,9,9,9,25,0,0,24,24,24,24,24,24,24,24,36,24,24,24,24,36,38,17,17,17,26,9,9,9,9,9,9,26,0,0,9,9,9,9,9,9,9,9,36,9,9,9,9,36,38,17,17,17,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,25,9,9,9,9,36,38,17,17,17,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,26,9,9,9,9,36,35,35,35,35,35,35,35,35,35,35,35,38,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,36,23,23,23,23,23,23,23,23,23,23,23,38,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,36,24,24,24,24,24,24,24,24,24,24,24,35,35,35,35,9,9,9,9,35,35,35,35,35,35,35,35,35,39,39,39,39,39,39,39,39,39,39,39,24,24,24,24,40,40,40,40,24,24,24,24,24,24,24,24,24]"); // `map_data`
                insertPs.setString(4, "2000-01-01"); // `date_expired`: bạn có thể thay đổi thành giá trị ngày tháng hợp lệ

                int rowsInserted = insertPs.executeUpdate();
                if (rowsInserted > 0) {
                    this.user.session.avatarService.serverDialog("Bạn đã mua nhà thành công!");


                }
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }


    }





    public void closeMessage() {
        if (this.isConnected()) {
            if (this.messageHandler != null) {
                this.messageHandler.onDisconnected();
            }
            this.close();
        }
    }

    public void changePassword(Message ms) throws IOException {
        String passOld = ms.reader().readUTF();
        String passNew = ms.reader().readUTF();
        try (Connection connection = DbManager.getInstance().getConnection()) {
            String ACCOUNT_LOGIN = "SELECT * FROM `users` WHERE `id` = ? AND `password` = ? LIMIT 1";
            PreparedStatement ps = connection.prepareStatement(ACCOUNT_LOGIN);
            ps.setInt(1, this.user.getId());
            ps.setString(2, Utils.md5(passOld));
            ResultSet red = ps.executeQuery();
            if (red.next()) {
                String ACCOUNT_UPDATE_PASSWORD = "UPDATE `users` SET `password` = ? WHERE `id` = ?";
                PreparedStatement changePass = connection.prepareStatement(ACCOUNT_UPDATE_PASSWORD);
                changePass.setString(1, Utils.md5(passNew));
                changePass.setInt(2, this.user.getId());
                int result = changePass.executeUpdate();
                if (result > 0) {
                    ms = new Message(-62);
                    DataOutputStream ds = ms.writer();
                    ds.writeUTF(passNew);
                    ds.flush();
                    this.sendMessage(ms);
                    this.user.getService().serverDialog("\u0110\u1ed5i m\u1eadt kh\u1ea9u th\u00e0nh c\u00f4ng.");
                } else {
                    this.user.getAvatarService()
                            .serverDialog("C\u00f3 l\u1ed7i x\u1ea3y ra, vui l\u00f2ng th\u1eed l\u1ea1i sau.");
                }
                changePass.close();
            } else {
                this.user.getService().serverDialog("M\u1eadt kh\u1ea9u c\u0169 kh\u00f4ng \u0111\u00fang.");
            }
            red.close();
            ps.close();
        } catch (SQLException e) {
            e.printStackTrace();
            System.out.println(e.getMessage());
        }
    }

    private class Sender implements Runnable {

        private Deque<Message> sendingMessage;

        public Sender() {
            this.sendingMessage = new ArrayDeque<Message>();
        }

        public void AddMessage(Message message) {
            this.sendingMessage.add(message);
        }

        @Override
        public void run() {
            while (isConnected()) {
                while (!this.sendingMessage.isEmpty()) {
                    Message message = this.sendingMessage.poll();
                    doSendMessage(message);
                }
                try {
                    Thread.sleep(10L);
                } catch (InterruptedException ex) {
                }
            }
        }
    }

    class MessageCollector implements Runnable {

        @Override
        public void run() {
            try {
                while (true) {
                    Message message = this.readMessage();
                    if (message == null) {
                        break;
                    }
                    messageHandler.onMessage(message);
                    message.cleanup();
                }
            } catch (Exception ex) {
            }
            if (isConnected()) {
                if (messageHandler != null) {
                    messageHandler.onDisconnected();
                }
                close();
            }
        }

        private Message readMessage() {
            try {
                byte cmd = dis.readByte();
                if (connected) {
                    cmd = readKey(cmd);
                }
                int size;
                if (connected) {
                    byte b1 = dis.readByte();
                    byte b2 = dis.readByte();
                    size = ((readKey(b1) & 0xFF) << 8 | (readKey(b2) & 0xFF));
                } else {
                    size = dis.readUnsignedShort();
                }
                byte[] data = new byte[size];
                for (int len = 0, byteRead = 0; len != -1 && byteRead < size; byteRead += len) {
                    len = dis.read(data, byteRead, size - byteRead);
                    if (len > 0) {
                    }
                }
                if (connected) {
                    for (int i = 0; i < data.length; ++i) {
                        data[i] = readKey(data[i]);
                    }
                }
                Message msg = new Message(cmd, data);
                return msg;
            } catch (Exception e) {
            }
            return null;
        }
    }
}