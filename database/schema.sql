-- =====================================================
-- ERP-System | MySQL 8 şeması
-- Modüller: Auth, Ürün/Stok, Satış, Satın Alma
-- =====================================================

CREATE DATABASE IF NOT EXISTS erp_system
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE erp_system;

-- ---------- 1. AUTH ----------

CREATE TABLE roles (
  id    INT AUTO_INCREMENT PRIMARY KEY,
  name  VARCHAR(50) NOT NULL UNIQUE          -- Admin, Satis, Depo
) ENGINE=InnoDB;

CREATE TABLE users (
  id            INT AUTO_INCREMENT PRIMARY KEY,
  username      VARCHAR(50)  NOT NULL UNIQUE,
  email         VARCHAR(150) NOT NULL UNIQUE,
  password_hash VARCHAR(255) NOT NULL,       -- düz şifre ASLA saklanmaz
  role_id       INT NOT NULL,
  is_active     TINYINT(1) NOT NULL DEFAULT 1,  -- silme yok, pasifleştirme var
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_users_role FOREIGN KEY (role_id) REFERENCES roles(id)
) ENGINE=InnoDB;

-- ---------- 2. ÜRÜN VE STOK ----------

CREATE TABLE categories (
  id         INT AUTO_INCREMENT PRIMARY KEY,
  name       VARCHAR(100) NOT NULL,
  parent_id  INT NULL,                        -- alt kategori için (opsiyonel)
  CONSTRAINT fk_categories_parent FOREIGN KEY (parent_id) REFERENCES categories(id)
) ENGINE=InnoDB;

CREATE TABLE products (
  id              INT AUTO_INCREMENT PRIMARY KEY,
  sku             VARCHAR(50)  NOT NULL UNIQUE,   -- ürün kodu, benzersiz
  name            VARCHAR(200) NOT NULL,
  category_id     INT NOT NULL,
  unit            VARCHAR(20)  NOT NULL DEFAULT 'adet',
  sale_price      DECIMAL(18,2) NOT NULL DEFAULT 0,
  purchase_price  DECIMAL(18,2) NOT NULL DEFAULT 0,
  min_stock_level DECIMAL(18,3) NOT NULL DEFAULT 0,
  is_active       TINYINT(1) NOT NULL DEFAULT 1,
  created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_products_category FOREIGN KEY (category_id) REFERENCES categories(id),
  CONSTRAINT chk_products_prices CHECK (sale_price >= 0 AND purchase_price >= 0)
) ENGINE=InnoDB;

-- Stok tek sütun değil, hareketlerin toplamıdır. Giriş +, çıkış -.
CREATE TABLE stock_movements (
  id             BIGINT AUTO_INCREMENT PRIMARY KEY,
  product_id     INT NOT NULL,
  quantity       DECIMAL(18,3) NOT NULL,          -- giriş +, çıkış -
  movement_type  ENUM('PurchaseReceipt','SaleOut','SaleCancel','Adjustment','Return') NOT NULL,
  reference_type ENUM('SalesOrder','PurchaseOrder') NULL,
  reference_id   INT NULL,
  note           VARCHAR(255) NULL,
  created_by     INT NOT NULL,
  created_at     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_stock_product FOREIGN KEY (product_id) REFERENCES products(id),
  CONSTRAINT fk_stock_user    FOREIGN KEY (created_by) REFERENCES users(id),
  CONSTRAINT chk_stock_qty    CHECK (quantity <> 0),
  INDEX idx_stock_product (product_id),
  INDEX idx_stock_reference (reference_type, reference_id)
) ENGINE=InnoDB;

-- ---------- 3. SATIŞ ----------

CREATE TABLE customers (
  id         INT AUTO_INCREMENT PRIMARY KEY,
  name       VARCHAR(200) NOT NULL,
  tax_number VARCHAR(20)  NULL,
  email      VARCHAR(150) NULL,
  phone      VARCHAR(30)  NULL,
  address    VARCHAR(500) NULL,
  is_active  TINYINT(1) NOT NULL DEFAULT 1,
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE sales_orders (
  id           INT AUTO_INCREMENT PRIMARY KEY,
  order_no     VARCHAR(30) NOT NULL UNIQUE,
  customer_id  INT NOT NULL,
  status       ENUM('Draft','Approved','Shipped','Cancelled') NOT NULL DEFAULT 'Draft',
  total_amount DECIMAL(18,2) NOT NULL DEFAULT 0,
  created_by   INT NOT NULL,
  order_date   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_so_customer FOREIGN KEY (customer_id) REFERENCES customers(id),
  CONSTRAINT fk_so_user     FOREIGN KEY (created_by)  REFERENCES users(id),
  INDEX idx_so_status (status),
  INDEX idx_so_customer (customer_id)
) ENGINE=InnoDB;

CREATE TABLE sales_order_items (
  id             INT AUTO_INCREMENT PRIMARY KEY,
  sales_order_id INT NOT NULL,
  product_id     INT NOT NULL,
  quantity       DECIMAL(18,3) NOT NULL,
  unit_price     DECIMAL(18,2) NOT NULL,          -- sipariş anındaki fiyat (kopya)
  line_total     DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_soi_order   FOREIGN KEY (sales_order_id) REFERENCES sales_orders(id) ON DELETE CASCADE,
  CONSTRAINT fk_soi_product FOREIGN KEY (product_id)     REFERENCES products(id),
  CONSTRAINT chk_soi_qty    CHECK (quantity > 0),
  INDEX idx_soi_order (sales_order_id)
) ENGINE=InnoDB;

-- ---------- 4. SATIN ALMA ----------

CREATE TABLE suppliers (
  id         INT AUTO_INCREMENT PRIMARY KEY,
  name       VARCHAR(200) NOT NULL,
  tax_number VARCHAR(20)  NULL,
  email      VARCHAR(150) NULL,
  phone      VARCHAR(30)  NULL,
  is_active  TINYINT(1) NOT NULL DEFAULT 1,
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE purchase_orders (
  id           INT AUTO_INCREMENT PRIMARY KEY,
  order_no     VARCHAR(30) NOT NULL UNIQUE,
  supplier_id  INT NOT NULL,
  status       ENUM('Draft','Ordered','Received','Cancelled') NOT NULL DEFAULT 'Draft',
  total_amount DECIMAL(18,2) NOT NULL DEFAULT 0,
  created_by   INT NOT NULL,
  order_date   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_po_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(id),
  CONSTRAINT fk_po_user     FOREIGN KEY (created_by)  REFERENCES users(id),
  INDEX idx_po_status (status),
  INDEX idx_po_supplier (supplier_id)
) ENGINE=InnoDB;

CREATE TABLE purchase_order_items (
  id                INT AUTO_INCREMENT PRIMARY KEY,
  purchase_order_id INT NOT NULL,
  product_id        INT NOT NULL,
  quantity          DECIMAL(18,3) NOT NULL,
  unit_price        DECIMAL(18,2) NOT NULL,
  line_total        DECIMAL(18,2) NOT NULL,
  CONSTRAINT fk_poi_order   FOREIGN KEY (purchase_order_id) REFERENCES purchase_orders(id) ON DELETE CASCADE,
  CONSTRAINT fk_poi_product FOREIGN KEY (product_id)        REFERENCES products(id),
  CONSTRAINT chk_poi_qty    CHECK (quantity > 0),
  INDEX idx_poi_order (purchase_order_id)
) ENGINE=InnoDB;

-- ---------- SEED (başlangıç verisi) ----------

INSERT INTO roles (name) VALUES ('Admin'), ('Satis'), ('Depo');