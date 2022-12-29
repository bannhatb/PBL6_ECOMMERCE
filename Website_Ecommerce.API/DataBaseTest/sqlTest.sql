-- Create DB Roles
insert into Roles values (1, "Admin");
insert into Roles values (2, "Shop");
insert into Roles values (3, "Customer");
insert into Roles values (4, "Shipper");

-- DB category
insert into Categories values (1, "Vest");
insert into Categories values (2, "Giày");
insert into Categories values (3, "Quầy Tây");
-- Chạy API => API Register
-- DB shop
insert into Shops values (1, "ABC", 3, "Hà Nội", "ABC@gmail.com", "http:///", "1234", 5, 4.6, 250, 1);
insert into Shops values (2, "DEF", 4, "Hà Nội", "DEF@gmail.com", "http:///", "1234", 5, 4.6, 250, 1);
insert into Shops values (3, "GHI", 7, "ĐÀ NẴNG", "GHI@gmail.com", "http:///", "1234", 5, 4.6, 250, 1);

-- DB Product

INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('1', 'Bộ vest nam 2 khuy, áo vest 1 hàng 2 cúc, hàng 2 lớp lót đệm đầy đủ, quần âu áo vest form chuẩn cao cấp', ' casimia', 'Việt Nam', 'Sản phẩm có sẵn tại cửa hàng quý khách có thể tham khảo mua trực tiếp tại shop, Bộ Vest Nam Đen Hàn Quốc 6 Khuy, 2 Lớp Cao Cấp', '1', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('2', 'Áo blazer nam chumi 2 lớp dày dặn form rộng dáng unisex ad006', 'Vải Wool', 'Việt Nam', 'Áo blazer nam chumi  2 lớp dày dặn form rộng dáng unisex ad006', '1', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('3', 'Giày Thể Thao Sneaker Nam - Dây Kẻ Guccc Siêu Đẹp GA022', 'Sợi tổng hợp', 'Việt Nam', 'Giày Thể Thao Sneaker Nam - Dây Kẻ Guccc Siêu Đẹp GA022 ', '1', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('4', 'LV 10.10 XẢ KHO Giày Da Nam BH02. Màu Nâu- Đen, Da Bò. | HOT TREND | 2020 new RẺ ĐẸP ', 'Da', 'Việt Nam', 'Giày Da Nam BH02, sản xuất trong nước, với chất liệu da bò chuẩn, 2 màu nâu- đen dễ kết hợp trang phục.', '1', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('5', 'Có 2 Màu) Giày lười nam da PU đế khâu siêu chất', 'Da', 'Việt Nam', 'Đế giày được thiết kế chịu ma sát tốt, , nhẹ, êm, cân bằng và thoáng khí ', '2', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('6', 'Quần tây nam hàn quốc dáng baggy suông đen, kem vải dày dặn co giãn tốt thương hiệu JBAGY - JA0101', 'Chất liệu: Cot Hàn ', 'Việt Nam ', 'Bạn muốn chọn 1 chiếc quần vải kém chất lượng về không mặc nổi hay chọn 1 chiếc quần cao cấp với chất lượng hoàn toàn xứng đáng giúp bạn thoải mái và tự tin?', '2', '1');
INSERT INTO `PBL6_ECOMMERCE`.`Products` (`Id`, `Name`, `Material`, `Origin`, `Description`, `ShopId`, `Status`) VALUES ('7', 'Quần Âu Nam - Quần Tây Nam Trendyman Công Sở Đen Cá Tính Dáng Ôm Chuẩn Vải Mỏng Nhẹ Không Nhăn Không Xù', 'Sợi dệt, lụa', 'Việt nam', 'Trendyman xin phép giới thiệu đến các bạn mẫu sản phẩm mới: Quần Tây Nam - quần nam Trendyman Công Sở Đen Cá Tính Dáng Ôm Chuẩn Vải mỏng nhẹ Không Nhăn không xù', '2', '1');


-- DB ProductDetail

INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('1', '1', '41', 'Đen', '100', '450000', '500000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('2', '1', '42', 'Đen', '100', '390000', '500000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('3', '1', '41', 'Xanh Đen', '100', '450000', '500000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('4', '1', '42', ' Nâu ki', '100', '390000', '450000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('5', '2', '40', 'Đen', '100', '250000', '290000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('6', '2', '41', 'Đen', '100', '250000', '300000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('7', '2', '40', 'Nâu', '90', '250000', '350000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('8', '4', '40', 'Đen', '98', '190000', '350000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('9', '4', '41', 'Đen', '98', '200000', '290000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('10', '7', '39', 'Đen', '100', '250000', '310000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('11', '7', '40', 'Đen', '100', '250000', '300000');
INSERT INTO `PBL6_ECOMMERCE`.`ProductDetails` (`Id`, `ProductId`, `Size`, `Color`, `Amount`, `Price`, `InitialPrice`) VALUES ('12', '7', '40', 'Trắng', '190', '240000', '300000');

-- DB Product Image
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('1', '1', 'https://cf.shopee.vn/file/5e243f481e1cdb72ba26a8e690cf1a04');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('2', '2', 'https://cf.shopee.vn/file/d195137ba7ed532d739290ba3b941c18');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('3', '3', 'https://cf.shopee.vn/file/7d59e6e23b5adca37a721660a2d0804a');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('4', '4', 'https://cf.shopee.vn/file/39ba55af4ef2c88080163ecb7a478a13');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('5', '5', 'https://cf.shopee.vn/file/1b8625fd4c19eee0b362b652b2bee164');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('6', '6', 'https://cf.shopee.vn/file/671f23e96805061b15fd7dbed10ec602');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('7', '7', 'https://cf.shopee.vn/file/0f9275e2c65ce8a0e79c57c79a447622');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('8', '8', 'https://cf.shopee.vn/file/d5a4b11a4c1c19886b4a32ccbfaaa5f5');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('9', '9', 'https://cf.shopee.vn/file/4ef706aa3b971a3a692c49a282b99bdb');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('10', '10', 'https://cf.shopee.vn/file/9f79d99aa455d0519f3dd267e1dbc7c5');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('11', '11', 'https://cf.shopee.vn/file/2f6053893ea94ceac21021913dc031d0');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('12', '12', 'https://cf.shopee.vn/file/c6c0bc7acb9f4534fbc98df1a355456e');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('13', '9', 'https://cf.shopee.vn/file/31c873ace80a422bc01ae7a3acb57cde');
INSERT INTO `PBL6_ECOMMERCE`.`ProductImages` (`Id`, `ProductDetailId`, `UrlImage`) VALUES ('14', '9', 'https://cf.shopee.vn/file/4219c6940677e5f5b3486256217d8717');


-- DB voucher order

INSERT INTO `PBL6_ECOMMERCE`.`VoucherOrders` (`Id`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('1', '15000', '50000', '100', '2022:11:17 12:12:12', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherOrders` (`Id`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('2', '2000', '15000', '1000', '2022-11-17 12:12:12.000000', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherOrders` (`Id`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('3', '7000', '20000', '1000', '2022-11-17 12:12:12.000000', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherOrders` (`Id`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('4', '10000', '70000', '1000', '2022-10-17 12:12:12.000000', '2022-11-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherOrders` (`Id`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('5', '4000', '29000', '1000', '2022-12-17 12:12:12.000000', '2023-11-17 12:12:12.000000');


-- DB voucher shop
INSERT INTO `PBL6_ECOMMERCE`.`VoucherProducts` (`Id`, `ShopId`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('1', '1', '10000.0', '10000', '100', '2022-10-17 12:12:12.000000', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherProducts` (`Id`, `ShopId`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('2', '1', '9000', '10000', '100', '2022-8-17 12:12:12.000000', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherProducts` (`Id`, `ShopId`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('3', '1', '12000', '10000', '100', '2022-6-17 12:12:12.000000', '2022-11-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherProducts` (`Id`, `ShopId`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('4', '2', '12000', '50000', '1000', '2022-9-17 12:12:12.000000', '2022-12-17 12:12:12.000000');
INSERT INTO `PBL6_ECOMMERCE`.`VoucherProducts` (`Id`, `ShopId`, `Value`, `MinPrice`, `Amount`, `CreateAt`, `Expired`) VALUES ('5', '2', '11000', '10000', '900', '2022-7-17 12:12:12.000000', '2022-11-17 12:12:12.000000');


