/* ============================================================
   INFINITY PARTS — products.js
   Base de dados de produtos (fictícia, 150+ itens em 15
   categorias + linha "PC Montado") e funções utilitárias de
   cálculo de preço, renderização de cards e busca.
   ============================================================ */

const FALLBACK_PRODUCTS = [
  {
    "id": 1,
    "name": "RTX 4060 8GB",
    "brand": "NVIDIA",
    "category": "Placa de Vídeo",
    "price": 2499.9,
    "salePrice": 2199.9,
    "stock": 8,
    "rating": 4.8,
    "reviews": 37,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-GPU-001",
    "desc": "RTX 4060 8GB da NVIDIA, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "8GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 2,
    "name": "RTX 4060 Ti 16GB",
    "brand": "NVIDIA",
    "category": "Placa de Vídeo",
    "price": 3299.9,
    "salePrice": 2999.9,
    "stock": 6,
    "rating": 4.8,
    "reviews": 19,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-GPU-002",
    "desc": "RTX 4060 Ti 16GB da NVIDIA, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "16GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 3,
    "name": "RTX 4070 12GB",
    "brand": "NVIDIA",
    "category": "Placa de Vídeo",
    "price": 3899.9,
    "salePrice": 3499.9,
    "stock": 5,
    "rating": 4.9,
    "reviews": 21,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-GPU-003",
    "desc": "RTX 4070 12GB da NVIDIA, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "12GB GDDR6X",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 16-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 4,
    "name": "RTX 4070 Ti Super 16GB",
    "brand": "NVIDIA",
    "category": "Placa de Vídeo",
    "price": 5799.9,
    "salePrice": 5299.9,
    "stock": 3,
    "rating": 4.9,
    "reviews": 9,
    "image": "",
    "badges": [
      "ÚLTIMAS UNIDADES"
    ],
    "sku": "IP-GPU-004",
    "desc": "RTX 4070 Ti Super 16GB da NVIDIA, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "16GB GDDR6X",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 16-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 5,
    "name": "ROG Strix RTX 3060 12GB Gaming",
    "brand": "ASUS",
    "category": "Placa de Vídeo",
    "price": 2599.9,
    "salePrice": 2299.9,
    "stock": 7,
    "rating": 4.8,
    "reviews": 64,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-GPU-005",
    "desc": "ROG Strix RTX 3060 12GB Gaming da ASUS, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "12GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 6,
    "name": "RTX 5090 Gaming OC 32GB",
    "brand": "Gigabyte",
    "category": "Placa de Vídeo",
    "price": 14999.9,
    "salePrice": 13999.9,
    "stock": 2,
    "rating": 5.0,
    "reviews": 6,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-GPU-006",
    "desc": "RTX 5090 Gaming OC 32GB da Gigabyte, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "32GB GDDR7",
      "Interface": "PCI Express 5.0",
      "Conector": "1x 16-pin",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 7,
    "name": "RX 7600 8GB",
    "brand": "AMD",
    "category": "Placa de Vídeo",
    "price": 1999.9,
    "salePrice": 1799.9,
    "stock": 12,
    "rating": 4.6,
    "reviews": 15,
    "image": "",
    "badges": [],
    "sku": "IP-GPU-007",
    "desc": "RX 7600 8GB da AMD, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "8GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 8,
    "name": "RX 7700 XT 12GB",
    "brand": "AMD",
    "category": "Placa de Vídeo",
    "price": 3299.9,
    "salePrice": 2999.9,
    "stock": 6,
    "rating": 4.7,
    "reviews": 11,
    "image": "",
    "badges": [],
    "sku": "IP-GPU-008",
    "desc": "RX 7700 XT 12GB da AMD, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "12GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 9,
    "name": "RX 7800 XT 16GB",
    "brand": "AMD",
    "category": "Placa de Vídeo",
    "price": 3999.9,
    "salePrice": 3599.9,
    "stock": 4,
    "rating": 4.8,
    "reviews": 8,
    "image": "",
    "badges": [],
    "sku": "IP-GPU-009",
    "desc": "RX 7800 XT 16GB da AMD, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "16GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 10,
    "name": "Arc A750 8GB",
    "brand": "Intel",
    "category": "Placa de Vídeo",
    "price": 1799.9,
    "salePrice": 1599.9,
    "stock": 0,
    "rating": 4.3,
    "reviews": 5,
    "image": "",
    "badges": [
      "ESGOTADO"
    ],
    "sku": "IP-GPU-010",
    "desc": "Arc A750 8GB da Intel, ideal para quem busca desempenho e qualidade em placa de vídeo.",
    "specs": {
      "Memória": "8GB GDDR6",
      "Interface": "PCI Express 4.0",
      "Conector": "1x 8-pin",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 11,
    "name": "Ryzen 5 5600",
    "brand": "AMD",
    "category": "Processador",
    "price": 899.9,
    "salePrice": 749.9,
    "stock": 20,
    "rating": 4.9,
    "reviews": 88,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-CPU-011",
    "desc": "Ryzen 5 5600 da AMD, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "6",
      "Threads": "12",
      "Clock Base": "3.5GHz",
      "Socket": "AM4",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 12,
    "name": "Ryzen 7 5700X",
    "brand": "AMD",
    "category": "Processador",
    "price": 1199.9,
    "salePrice": 999.9,
    "stock": 14,
    "rating": 4.8,
    "reviews": 40,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-012",
    "desc": "Ryzen 7 5700X da AMD, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "8",
      "Threads": "16",
      "Clock Base": "3.4GHz",
      "Socket": "AM4",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 13,
    "name": "Ryzen 5 7600",
    "brand": "AMD",
    "category": "Processador",
    "price": 1499.9,
    "salePrice": 1299.9,
    "stock": 10,
    "rating": 4.8,
    "reviews": 22,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-CPU-013",
    "desc": "Ryzen 5 7600 da AMD, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "6",
      "Threads": "12",
      "Clock Base": "3.8GHz",
      "Socket": "AM5",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 14,
    "name": "Ryzen 7 7700X",
    "brand": "AMD",
    "category": "Processador",
    "price": 2199.9,
    "salePrice": 1949.9,
    "stock": 6,
    "rating": 4.8,
    "reviews": 24,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-014",
    "desc": "Ryzen 7 7700X da AMD, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "8",
      "Threads": "16",
      "Clock Base": "4.5GHz",
      "Socket": "AM5",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 15,
    "name": "Ryzen 9 7900X",
    "brand": "AMD",
    "category": "Processador",
    "price": 3299.9,
    "salePrice": 2949.9,
    "stock": 4,
    "rating": 4.9,
    "reviews": 13,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-CPU-015",
    "desc": "Ryzen 9 7900X da AMD, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "12",
      "Threads": "24",
      "Clock Base": "4.7GHz",
      "Socket": "AM5",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 16,
    "name": "Core i5-12400F",
    "brand": "Intel",
    "category": "Processador",
    "price": 999.9,
    "salePrice": 849.9,
    "stock": 16,
    "rating": 4.7,
    "reviews": 55,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-016",
    "desc": "Core i5-12400F da Intel, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "6",
      "Threads": "12",
      "Clock Base": "2.5GHz",
      "Socket": "LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 17,
    "name": "Core i5-13400F",
    "brand": "Intel",
    "category": "Processador",
    "price": 1299.9,
    "salePrice": 1099.9,
    "stock": 15,
    "rating": 4.7,
    "reviews": 41,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-017",
    "desc": "Core i5-13400F da Intel, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "10",
      "Threads": "16",
      "Clock Base": "2.5GHz",
      "Socket": "LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 18,
    "name": "Core i7-12700F",
    "brand": "Intel",
    "category": "Processador",
    "price": 1999.9,
    "salePrice": 1749.9,
    "stock": 8,
    "rating": 4.8,
    "reviews": 19,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-018",
    "desc": "Core i7-12700F da Intel, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "12",
      "Threads": "20",
      "Clock Base": "2.1GHz",
      "Socket": "LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 19,
    "name": "Core i7-13700K",
    "brand": "Intel",
    "category": "Processador",
    "price": 2999.9,
    "salePrice": 2699.9,
    "stock": 4,
    "rating": 4.9,
    "reviews": 12,
    "image": "",
    "badges": [
      "ÚLTIMAS UNIDADES"
    ],
    "sku": "IP-CPU-019",
    "desc": "Core i7-13700K da Intel, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "16",
      "Threads": "24",
      "Clock Base": "3.4GHz",
      "Socket": "LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 20,
    "name": "Core i9-10900",
    "brand": "Intel",
    "category": "Processador",
    "price": 2199.9,
    "salePrice": 1899.9,
    "stock": 5,
    "rating": 4.6,
    "reviews": 17,
    "image": "",
    "badges": [],
    "sku": "IP-CPU-020",
    "desc": "Core i9-10900 da Intel, ideal para quem busca desempenho e qualidade em processador.",
    "specs": {
      "Núcleos": "10",
      "Threads": "20",
      "Clock Base": "2.8GHz",
      "Socket": "LGA1200",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 21,
    "name": "Prime B550M-A",
    "brand": "ASUS",
    "category": "Placa-mãe",
    "price": 799.9,
    "salePrice": 649.9,
    "stock": 10,
    "rating": 4.6,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-021",
    "desc": "Prime B550M-A da ASUS, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM4",
      "Chipset": "B550",
      "Formato": "Micro ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 22,
    "name": "ROG Strix B550-F Gaming",
    "brand": "ASUS",
    "category": "Placa-mãe",
    "price": 1499.9,
    "salePrice": 1299.9,
    "stock": 6,
    "rating": 4.8,
    "reviews": 20,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-MBD-022",
    "desc": "ROG Strix B550-F Gaming da ASUS, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM4",
      "Chipset": "B550",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 23,
    "name": "B650 Gaming X",
    "brand": "Gigabyte",
    "category": "Placa-mãe",
    "price": 1399.9,
    "salePrice": 1199.9,
    "stock": 7,
    "rating": 4.7,
    "reviews": 18,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-023",
    "desc": "B650 Gaming X da Gigabyte, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM5",
      "Chipset": "B650",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 24,
    "name": "Z790 Gaming X",
    "brand": "Gigabyte",
    "category": "Placa-mãe",
    "price": 2299.9,
    "salePrice": 1999.9,
    "stock": 4,
    "rating": 4.8,
    "reviews": 9,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-024",
    "desc": "Z790 Gaming X da Gigabyte, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "LGA1700",
      "Chipset": "Z790",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 25,
    "name": "MAG B760 Tomahawk WiFi",
    "brand": "MSI",
    "category": "Placa-mãe",
    "price": 1699.9,
    "salePrice": 1499.9,
    "stock": 8,
    "rating": 4.8,
    "reviews": 26,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-MBD-025",
    "desc": "MAG B760 Tomahawk WiFi da MSI, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "LGA1700",
      "Chipset": "B760",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 26,
    "name": "B650 Gaming Plus WiFi",
    "brand": "MSI",
    "category": "Placa-mãe",
    "price": 1399.9,
    "salePrice": 1199.9,
    "stock": 7,
    "rating": 4.7,
    "reviews": 18,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-026",
    "desc": "B650 Gaming Plus WiFi da MSI, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM5",
      "Chipset": "B650",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 27,
    "name": "MAG B550 Tomahawk",
    "brand": "MSI",
    "category": "Placa-mãe",
    "price": 1199.9,
    "salePrice": 999.9,
    "stock": 9,
    "rating": 4.7,
    "reviews": 31,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-027",
    "desc": "MAG B550 Tomahawk da MSI, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM4",
      "Chipset": "B550",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 28,
    "name": "PRO Z690-A",
    "brand": "MSI",
    "category": "Placa-mãe",
    "price": 1899.9,
    "salePrice": 1649.9,
    "stock": 3,
    "rating": 4.7,
    "reviews": 7,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-028",
    "desc": "PRO Z690-A da MSI, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "LGA1700",
      "Chipset": "Z690",
      "Formato": "ATX",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 29,
    "name": "B450M Pro4",
    "brand": "ASRock",
    "category": "Placa-mãe",
    "price": 549.9,
    "salePrice": 449.9,
    "stock": 14,
    "rating": 4.5,
    "reviews": 42,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-029",
    "desc": "B450M Pro4 da ASRock, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "AM4",
      "Chipset": "B450",
      "Formato": "Micro ATX",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 30,
    "name": "B760MH",
    "brand": "Biostar",
    "category": "Placa-mãe",
    "price": 649.9,
    "salePrice": 549.9,
    "stock": 11,
    "rating": 4.4,
    "reviews": 15,
    "image": "",
    "badges": [],
    "sku": "IP-MBD-030",
    "desc": "B760MH da Biostar, ideal para quem busca desempenho e qualidade em placa-mãe.",
    "specs": {
      "Socket": "LGA1700",
      "Chipset": "B760",
      "Formato": "Micro ATX",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 31,
    "name": "Fury Beast 8GB DDR4 3200MHz",
    "brand": "Kingston",
    "category": "Memória RAM",
    "price": 189.9,
    "salePrice": 149.9,
    "stock": 40,
    "rating": 4.7,
    "reviews": 90,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-031",
    "desc": "Fury Beast 8GB DDR4 3200MHz da Kingston, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "8GB",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 32,
    "name": "Fury Beast 16GB (2x8GB) DDR4 3200MHz",
    "brand": "Kingston",
    "category": "Memória RAM",
    "price": 349.9,
    "salePrice": 279.9,
    "stock": 30,
    "rating": 4.8,
    "reviews": 120,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-RAM-032",
    "desc": "Fury Beast 16GB (2x8GB) DDR4 3200MHz da Kingston, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "16GB (2x8GB)",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 33,
    "name": "Fury Beast 32GB (2x16GB) DDR4 3200MHz",
    "brand": "Kingston",
    "category": "Memória RAM",
    "price": 699.9,
    "salePrice": 599.9,
    "stock": 15,
    "rating": 4.8,
    "reviews": 38,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-033",
    "desc": "Fury Beast 32GB (2x16GB) DDR4 3200MHz da Kingston, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "32GB (2x16GB)",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 34,
    "name": "Vengeance RGB Pro 16GB (2x8GB) Branco DDR4 3200MHz",
    "brand": "Corsair",
    "category": "Memória RAM",
    "price": 429.9,
    "salePrice": 359.9,
    "stock": 12,
    "rating": 4.9,
    "reviews": 27,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-RAM-034",
    "desc": "Vengeance RGB Pro 16GB (2x8GB) Branco DDR4 3200MHz da Corsair, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "16GB (2x8GB)",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 35,
    "name": "Vengeance RGB Pro TUF 32GB (2x16GB) DDR4 3200MHz",
    "brand": "Corsair",
    "category": "Memória RAM",
    "price": 999.9,
    "salePrice": 849.9,
    "stock": 14,
    "rating": 4.9,
    "reviews": 27,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-035",
    "desc": "Vengeance RGB Pro TUF 32GB (2x16GB) DDR4 3200MHz da Corsair, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "32GB (2x16GB)",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 36,
    "name": "Vengeance LPX 8GB DDR4 3000MHz",
    "brand": "Corsair",
    "category": "Memória RAM",
    "price": 199.9,
    "salePrice": 169.9,
    "stock": 25,
    "rating": 4.6,
    "reviews": 71,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-036",
    "desc": "Vengeance LPX 8GB DDR4 3000MHz da Corsair, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "8GB",
      "Frequência": "3000MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 37,
    "name": "Fury 16GB DDR4 3200MHz",
    "brand": "HyperX",
    "category": "Memória RAM",
    "price": 359.9,
    "salePrice": 299.9,
    "stock": 18,
    "rating": 4.6,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-037",
    "desc": "Fury 16GB DDR4 3200MHz da HyperX, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "16GB",
      "Frequência": "3200MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 38,
    "name": "T-Force 16GB DDR5 6000MHz",
    "brand": "TeamGroup",
    "category": "Memória RAM",
    "price": 599.9,
    "salePrice": 499.9,
    "stock": 10,
    "rating": 4.8,
    "reviews": 14,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-RAM-038",
    "desc": "T-Force 16GB DDR5 6000MHz da TeamGroup, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "16GB",
      "Frequência": "6000MHz",
      "Tipo": "DDR5",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 39,
    "name": "Spectrix D45 32GB (2x16GB) DDR5 6000MHz",
    "brand": "XPG",
    "category": "Memória RAM",
    "price": 999.9,
    "salePrice": 849.9,
    "stock": 8,
    "rating": 4.9,
    "reviews": 11,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-039",
    "desc": "Spectrix D45 32GB (2x16GB) DDR5 6000MHz da XPG, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "32GB (2x16GB)",
      "Frequência": "6000MHz",
      "Tipo": "DDR5",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 40,
    "name": "8GB DDR4 2666MHz",
    "brand": "Crucial",
    "category": "Memória RAM",
    "price": 169.9,
    "salePrice": 139.9,
    "stock": 35,
    "rating": 4.5,
    "reviews": 58,
    "image": "",
    "badges": [],
    "sku": "IP-RAM-040",
    "desc": "8GB DDR4 2666MHz da Crucial, ideal para quem busca desempenho e qualidade em memória ram.",
    "specs": {
      "Capacidade": "8GB",
      "Frequência": "2666MHz",
      "Tipo": "DDR4",
      "Garantia": "vitalícia"
    }
  },
  {
    "id": 41,
    "name": "NM620 1TB NVMe",
    "brand": "Lexar",
    "category": "SSD",
    "price": 549.9,
    "salePrice": 429.9,
    "stock": 25,
    "rating": 4.8,
    "reviews": 96,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-SSD-041",
    "desc": "NM620 1TB NVMe da Lexar, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "NVMe PCIe 3.0",
      "Leitura": "3300MB/s",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 42,
    "name": "NV2 250GB NVMe",
    "brand": "Kingston",
    "category": "SSD",
    "price": 199.9,
    "salePrice": 159.9,
    "stock": 40,
    "rating": 4.6,
    "reviews": 70,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-042",
    "desc": "NV2 250GB NVMe da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "250GB",
      "Interface": "NVMe PCIe 4.0",
      "Leitura": "3000MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 43,
    "name": "NV2 500GB NVMe",
    "brand": "Kingston",
    "category": "SSD",
    "price": 289.9,
    "salePrice": 229.9,
    "stock": 35,
    "rating": 4.7,
    "reviews": 82,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-043",
    "desc": "NV2 500GB NVMe da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "500GB",
      "Interface": "NVMe PCIe 4.0",
      "Leitura": "3500MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 44,
    "name": "NV2 1TB NVMe",
    "brand": "Kingston",
    "category": "SSD",
    "price": 549.9,
    "salePrice": 449.9,
    "stock": 28,
    "rating": 4.8,
    "reviews": 64,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-SSD-044",
    "desc": "NV2 1TB NVMe da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "NVMe PCIe 4.0",
      "Leitura": "3500MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 45,
    "name": "NV2 2TB NVMe",
    "brand": "Kingston",
    "category": "SSD",
    "price": 999.9,
    "salePrice": 849.9,
    "stock": 12,
    "rating": 4.8,
    "reviews": 21,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-045",
    "desc": "NV2 2TB NVMe da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "2TB",
      "Interface": "NVMe PCIe 4.0",
      "Leitura": "3500MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 46,
    "name": "A400 240GB SATA",
    "brand": "Kingston",
    "category": "SSD",
    "price": 179.9,
    "salePrice": 139.9,
    "stock": 45,
    "rating": 4.5,
    "reviews": 130,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-046",
    "desc": "A400 240GB SATA da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "240GB",
      "Interface": "SATA III",
      "Leitura": "500MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 47,
    "name": "A400 480GB SATA",
    "brand": "Kingston",
    "category": "SSD",
    "price": 259.9,
    "salePrice": 199.9,
    "stock": 40,
    "rating": 4.6,
    "reviews": 143,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-047",
    "desc": "A400 480GB SATA da Kingston, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "480GB",
      "Interface": "SATA III",
      "Leitura": "550MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 48,
    "name": "Green 480GB SATA",
    "brand": "WD",
    "category": "SSD",
    "price": 249.9,
    "salePrice": 199.9,
    "stock": 30,
    "rating": 4.5,
    "reviews": 55,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-048",
    "desc": "Green 480GB SATA da WD, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "480GB",
      "Interface": "SATA III",
      "Leitura": "545MB/s",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 49,
    "name": "970 Evo 1TB NVMe",
    "brand": "Samsung",
    "category": "SSD",
    "price": 699.9,
    "salePrice": 599.9,
    "stock": 10,
    "rating": 4.9,
    "reviews": 24,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-SSD-049",
    "desc": "970 Evo 1TB NVMe da Samsung, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "NVMe PCIe 3.0",
      "Leitura": "3500MB/s",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 50,
    "name": "MX500 1TB SATA",
    "brand": "Crucial",
    "category": "SSD",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 16,
    "rating": 4.7,
    "reviews": 40,
    "image": "",
    "badges": [],
    "sku": "IP-SSD-050",
    "desc": "MX500 1TB SATA da Crucial, ideal para quem busca desempenho e qualidade em ssd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "SATA III",
      "Leitura": "560MB/s",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 51,
    "name": "WD Purple 2TB Surveillance",
    "brand": "Western Digital",
    "category": "HD",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 18,
    "rating": 4.7,
    "reviews": 61,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-051",
    "desc": "WD Purple 2TB Surveillance da Western Digital, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "2TB",
      "Interface": "SATA III",
      "RPM": "5400",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 52,
    "name": "Barracuda 1TB",
    "brand": "Seagate",
    "category": "HD",
    "price": 279.9,
    "salePrice": 229.9,
    "stock": 30,
    "rating": 4.5,
    "reviews": 88,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-052",
    "desc": "Barracuda 1TB da Seagate, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 53,
    "name": "Barracuda 2TB",
    "brand": "Seagate",
    "category": "HD",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 22,
    "rating": 4.6,
    "reviews": 54,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-HDD-053",
    "desc": "Barracuda 2TB da Seagate, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "2TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 54,
    "name": "Barracuda 4TB",
    "brand": "Seagate",
    "category": "HD",
    "price": 799.9,
    "salePrice": 699.9,
    "stock": 10,
    "rating": 4.7,
    "reviews": 29,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-054",
    "desc": "Barracuda 4TB da Seagate, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "4TB",
      "Interface": "SATA III",
      "RPM": "5400",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 55,
    "name": "Blue 1TB",
    "brand": "WD",
    "category": "HD",
    "price": 289.9,
    "salePrice": 239.9,
    "stock": 28,
    "rating": 4.5,
    "reviews": 70,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-055",
    "desc": "Blue 1TB da WD, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 56,
    "name": "Blue 2TB",
    "brand": "WD",
    "category": "HD",
    "price": 459.9,
    "salePrice": 389.9,
    "stock": 20,
    "rating": 4.6,
    "reviews": 46,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-056",
    "desc": "Blue 2TB da WD, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "2TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 57,
    "name": "P300 1TB",
    "brand": "Toshiba",
    "category": "HD",
    "price": 269.9,
    "salePrice": 219.9,
    "stock": 24,
    "rating": 4.4,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-057",
    "desc": "P300 1TB da Toshiba, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 58,
    "name": "IronWolf 4TB NAS",
    "brand": "Seagate",
    "category": "HD",
    "price": 999.9,
    "salePrice": 869.9,
    "stock": 6,
    "rating": 4.8,
    "reviews": 15,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-HDD-058",
    "desc": "IronWolf 4TB NAS da Seagate, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "4TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 59,
    "name": "Black 1TB Performance",
    "brand": "WD",
    "category": "HD",
    "price": 449.9,
    "salePrice": 389.9,
    "stock": 9,
    "rating": 4.7,
    "reviews": 19,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-059",
    "desc": "Black 1TB Performance da WD, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "1TB",
      "Interface": "SATA III",
      "RPM": "7200",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 60,
    "name": "Skyhawk 2TB Surveillance",
    "brand": "Seagate",
    "category": "HD",
    "price": 499.9,
    "salePrice": 429.9,
    "stock": 14,
    "rating": 4.6,
    "reviews": 22,
    "image": "",
    "badges": [],
    "sku": "IP-HDD-060",
    "desc": "Skyhawk 2TB Surveillance da Seagate, ideal para quem busca desempenho e qualidade em hd.",
    "specs": {
      "Capacidade": "2TB",
      "Interface": "SATA III",
      "RPM": "5900",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 61,
    "name": "B500 500W",
    "brand": "Imperion",
    "category": "Fonte",
    "price": 349.9,
    "salePrice": 279.9,
    "stock": 20,
    "rating": 4.5,
    "reviews": 48,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-PSU-061",
    "desc": "B500 500W da Imperion, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "500W",
      "Certificação": "Sem certificação",
      "Modular": "Não",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 62,
    "name": "CV550 550W 80 Plus Bronze",
    "brand": "Corsair",
    "category": "Fonte",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 18,
    "rating": 4.7,
    "reviews": 40,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-062",
    "desc": "CV550 550W 80 Plus Bronze da Corsair, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "550W",
      "Certificação": "80 Plus Bronze",
      "Modular": "Não",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 63,
    "name": "MAG A650BN 650W 80 Plus Bronze",
    "brand": "MSI",
    "category": "Fonte",
    "price": 599.9,
    "salePrice": 499.9,
    "stock": 16,
    "rating": 4.7,
    "reviews": 19,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-063",
    "desc": "MAG A650BN 650W 80 Plus Bronze da MSI, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "650W",
      "Certificação": "80 Plus Bronze",
      "Modular": "Não",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 64,
    "name": "600 BR 600W",
    "brand": "EVGA",
    "category": "Fonte",
    "price": 499.9,
    "salePrice": 419.9,
    "stock": 15,
    "rating": 4.6,
    "reviews": 31,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-064",
    "desc": "600 BR 600W da EVGA, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "600W",
      "Certificação": "80 Plus Bronze",
      "Modular": "Não",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 65,
    "name": "RM750x 750W 80 Plus Gold",
    "brand": "Corsair",
    "category": "Fonte",
    "price": 899.9,
    "salePrice": 779.9,
    "stock": 8,
    "rating": 4.9,
    "reviews": 22,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-PSU-065",
    "desc": "RM750x 750W 80 Plus Gold da Corsair, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "750W",
      "Certificação": "80 Plus Gold",
      "Modular": "Total",
      "Garantia": "120 meses"
    }
  },
  {
    "id": 66,
    "name": "Core Reactor 650W 80 Plus Gold",
    "brand": "XPG",
    "category": "Fonte",
    "price": 699.9,
    "salePrice": 599.9,
    "stock": 10,
    "rating": 4.8,
    "reviews": 14,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-066",
    "desc": "Core Reactor 650W 80 Plus Gold da XPG, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "650W",
      "Certificação": "80 Plus Gold",
      "Modular": "Total",
      "Garantia": "120 meses"
    }
  },
  {
    "id": 67,
    "name": "Squadra 500W",
    "brand": "Pichau",
    "category": "Fonte",
    "price": 299.9,
    "salePrice": 249.9,
    "stock": 25,
    "rating": 4.4,
    "reviews": 36,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-067",
    "desc": "Squadra 500W da Pichau, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "500W",
      "Certificação": "80 Plus",
      "Modular": "Não",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 68,
    "name": "Smart 500W",
    "brand": "Thermaltake",
    "category": "Fonte",
    "price": 329.9,
    "salePrice": 269.9,
    "stock": 22,
    "rating": 4.5,
    "reviews": 28,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-068",
    "desc": "Smart 500W da Thermaltake, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "500W",
      "Certificação": "80 Plus",
      "Modular": "Não",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 69,
    "name": "MWE 650 80 Plus Bronze",
    "brand": "Cooler Master",
    "category": "Fonte",
    "price": 599.9,
    "salePrice": 499.9,
    "stock": 12,
    "rating": 4.7,
    "reviews": 17,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-069",
    "desc": "MWE 650 80 Plus Bronze da Cooler Master, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "650W",
      "Certificação": "80 Plus Bronze",
      "Modular": "Semi",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 70,
    "name": "RGB 600W",
    "brand": "Redragon",
    "category": "Fonte",
    "price": 429.9,
    "salePrice": 359.9,
    "stock": 17,
    "rating": 4.4,
    "reviews": 25,
    "image": "",
    "badges": [],
    "sku": "IP-PSU-070",
    "desc": "RGB 600W da Redragon, ideal para quem busca desempenho e qualidade em fonte.",
    "specs": {
      "Potência": "600W",
      "Certificação": "80 Plus",
      "Modular": "Não",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 71,
    "name": "Diamondback RGB",
    "brand": "Redragon",
    "category": "Gabinete",
    "price": 349.9,
    "salePrice": 279.9,
    "stock": 22,
    "rating": 4.6,
    "reviews": 57,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-CSE-071",
    "desc": "Diamondback RGB da Redragon, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "3",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 72,
    "name": "TUF Gaming GT301",
    "brand": "ASUS",
    "category": "Gabinete",
    "price": 649.9,
    "salePrice": 549.9,
    "stock": 10,
    "rating": 4.8,
    "reviews": 22,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-CSE-072",
    "desc": "TUF Gaming GT301 da ASUS, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "3",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 73,
    "name": "C200 Glass",
    "brand": "Gigabyte",
    "category": "Gabinete",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 14,
    "rating": 4.5,
    "reviews": 19,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-073",
    "desc": "C200 Glass da Gigabyte, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "2",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 74,
    "name": "MasterBox Q300L",
    "brand": "Cooler Master",
    "category": "Gabinete",
    "price": 399.9,
    "salePrice": 329.9,
    "stock": 16,
    "rating": 4.6,
    "reviews": 41,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-074",
    "desc": "MasterBox Q300L da Cooler Master, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Micro ATX",
      "Lateral": "Acrílico",
      "Fans inclusos": "1",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 75,
    "name": "Mancer Gaming",
    "brand": "Pichau",
    "category": "Gabinete",
    "price": 299.9,
    "salePrice": 249.9,
    "stock": 20,
    "rating": 4.4,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-075",
    "desc": "Mancer Gaming da Pichau, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Acrílico",
      "Fans inclusos": "2",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 76,
    "name": "4000D Airflow",
    "brand": "Corsair",
    "category": "Gabinete",
    "price": 799.9,
    "salePrice": 699.9,
    "stock": 8,
    "rating": 4.9,
    "reviews": 26,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-CSE-076",
    "desc": "4000D Airflow da Corsair, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "2",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 77,
    "name": "Lancool 205",
    "brand": "Lian Li",
    "category": "Gabinete",
    "price": 549.9,
    "salePrice": 469.9,
    "stock": 9,
    "rating": 4.7,
    "reviews": 12,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-077",
    "desc": "Lancool 205 da Lian Li, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "3",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 78,
    "name": "Versa H18",
    "brand": "Thermaltake",
    "category": "Gabinete",
    "price": 329.9,
    "salePrice": 269.9,
    "stock": 18,
    "rating": 4.4,
    "reviews": 24,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-078",
    "desc": "Versa H18 da Thermaltake, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Acrílico",
      "Fans inclusos": "1",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 79,
    "name": "Star Scepter",
    "brand": "Redragon",
    "category": "Gabinete",
    "price": 379.9,
    "salePrice": 319.9,
    "stock": 15,
    "rating": 4.5,
    "reviews": 20,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-079",
    "desc": "Star Scepter da Redragon, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Vidro temperado",
      "Fans inclusos": "4",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 80,
    "name": "Compacto Slim ATX",
    "brand": "Pichau",
    "category": "Gabinete",
    "price": 249.9,
    "salePrice": 199.9,
    "stock": 24,
    "rating": 4.3,
    "reviews": 30,
    "image": "",
    "badges": [],
    "sku": "IP-CSE-080",
    "desc": "Compacto Slim ATX da Pichau, ideal para quem busca desempenho e qualidade em gabinete.",
    "specs": {
      "Formato": "Mid Tower",
      "Lateral": "Acrílico",
      "Fans inclusos": "1",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 81,
    "name": "Hyper 212 RGB Black Edition",
    "brand": "Cooler Master",
    "category": "Cooler",
    "price": 349.9,
    "salePrice": 279.9,
    "stock": 20,
    "rating": 4.7,
    "reviews": 63,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-CLR-081",
    "desc": "Hyper 212 RGB Black Edition da Cooler Master, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "159mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 82,
    "name": "AG400",
    "brand": "Deepcool",
    "category": "Cooler",
    "price": 199.9,
    "salePrice": 159.9,
    "stock": 26,
    "rating": 4.6,
    "reviews": 39,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-082",
    "desc": "AG400 da Deepcool, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "153mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 83,
    "name": "SE-224",
    "brand": "ID-Cooling",
    "category": "Cooler",
    "price": 179.9,
    "salePrice": 139.9,
    "stock": 22,
    "rating": 4.5,
    "reviews": 27,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-083",
    "desc": "SE-224 da ID-Cooling, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "154mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 84,
    "name": "Tyr",
    "brand": "Redragon",
    "category": "Cooler",
    "price": 149.9,
    "salePrice": 119.9,
    "stock": 30,
    "rating": 4.4,
    "reviews": 45,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-084",
    "desc": "Tyr da Redragon, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "150mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 85,
    "name": "iCUE H100i 240mm",
    "brand": "Corsair",
    "category": "Cooler",
    "price": 899.9,
    "salePrice": 779.9,
    "stock": 6,
    "rating": 4.9,
    "reviews": 18,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-CLR-085",
    "desc": "iCUE H100i 240mm da Corsair, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Radiador": "240mm",
      "Fans": "2x120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "60 meses"
    }
  },
  {
    "id": 86,
    "name": "Kraken X53 240mm",
    "brand": "NZXT",
    "category": "Cooler",
    "price": 849.9,
    "salePrice": 729.9,
    "stock": 5,
    "rating": 4.8,
    "reviews": 14,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-086",
    "desc": "Kraken X53 240mm da NZXT, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Radiador": "240mm",
      "Fans": "2x120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "72 meses"
    }
  },
  {
    "id": 87,
    "name": "UX200 ARGB",
    "brand": "Thermaltake",
    "category": "Cooler",
    "price": 219.9,
    "salePrice": 179.9,
    "stock": 18,
    "rating": 4.5,
    "reviews": 21,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-087",
    "desc": "UX200 ARGB da Thermaltake, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 88,
    "name": "Frost 120",
    "brand": "Pichau",
    "category": "Cooler",
    "price": 129.9,
    "salePrice": 99.9,
    "stock": 28,
    "rating": 4.3,
    "reviews": 32,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-088",
    "desc": "Frost 120 da Pichau, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Tipo": "Air Cooler",
      "Altura": "120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 89,
    "name": "Aquático 120mm",
    "brand": "PCYes",
    "category": "Cooler",
    "price": 349.9,
    "salePrice": 289.9,
    "stock": 10,
    "rating": 4.5,
    "reviews": 16,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-089",
    "desc": "Aquático 120mm da PCYes, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Radiador": "120mm",
      "Fans": "1x120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 90,
    "name": "MasterLiquid 120L",
    "brand": "Cooler Master",
    "category": "Cooler",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 8,
    "rating": 4.7,
    "reviews": 11,
    "image": "",
    "badges": [],
    "sku": "IP-CLR-090",
    "desc": "MasterLiquid 120L da Cooler Master, ideal para quem busca desempenho e qualidade em cooler.",
    "specs": {
      "Radiador": "120mm",
      "Fans": "1x120mm",
      "Compatibilidade": "AM4/AM5/LGA1700",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 91,
    "name": "GC-F007 RGB Kit 3un",
    "brand": "Redragon",
    "category": "Fan",
    "price": 249.9,
    "salePrice": 179.9,
    "stock": 30,
    "rating": 4.6,
    "reviews": 48,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-FAN-091",
    "desc": "GC-F007 RGB Kit 3un da Redragon, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "3 unidades",
      "Iluminação": "ARGB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 92,
    "name": "LL120 RGB Kit 2un",
    "brand": "Corsair",
    "category": "Fan",
    "price": 399.9,
    "salePrice": 329.9,
    "stock": 16,
    "rating": 4.8,
    "reviews": 22,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-092",
    "desc": "LL120 RGB Kit 2un da Corsair, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "2 unidades",
      "Iluminação": "RGB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 93,
    "name": "RF120",
    "brand": "Deepcool",
    "category": "Fan",
    "price": 79.9,
    "salePrice": 59.9,
    "stock": 40,
    "rating": 4.4,
    "reviews": 35,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-093",
    "desc": "RF120 da Deepcool, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "ARGB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 94,
    "name": "Aer RGB 2",
    "brand": "NZXT",
    "category": "Fan",
    "price": 149.9,
    "salePrice": 119.9,
    "stock": 22,
    "rating": 4.7,
    "reviews": 19,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-FAN-094",
    "desc": "Aer RGB 2 da NZXT, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "RGB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 95,
    "name": "SickleFlow 120",
    "brand": "Cooler Master",
    "category": "Fan",
    "price": 69.9,
    "salePrice": 49.9,
    "stock": 45,
    "rating": 4.5,
    "reviews": 60,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-FAN-095",
    "desc": "SickleFlow 120 da Cooler Master, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "ARGB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 96,
    "name": "Fan RGB 120mm",
    "brand": "Pichau",
    "category": "Fan",
    "price": 59.9,
    "salePrice": 44.9,
    "stock": 50,
    "rating": 4.3,
    "reviews": 44,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-096",
    "desc": "Fan RGB 120mm da Pichau, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "RGB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 97,
    "name": "Riing 12",
    "brand": "Thermaltake",
    "category": "Fan",
    "price": 99.9,
    "salePrice": 79.9,
    "stock": 28,
    "rating": 4.6,
    "reviews": 25,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-097",
    "desc": "Riing 12 da Thermaltake, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "RGB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 98,
    "name": "AORUS 120",
    "brand": "Gigabyte",
    "category": "Fan",
    "price": 119.9,
    "salePrice": 94.9,
    "stock": 20,
    "rating": 4.5,
    "reviews": 14,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-098",
    "desc": "AORUS 120 da Gigabyte, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "ARGB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 99,
    "name": "Vento 120",
    "brand": "XPG",
    "category": "Fan",
    "price": 89.9,
    "salePrice": 69.9,
    "stock": 24,
    "rating": 4.4,
    "reviews": 18,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-099",
    "desc": "Vento 120 da XPG, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "ARGB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 100,
    "name": "Vinik Fan 120",
    "brand": "PCYes",
    "category": "Fan",
    "price": 49.9,
    "salePrice": 34.9,
    "stock": 60,
    "rating": 4.2,
    "reviews": 51,
    "image": "",
    "badges": [],
    "sku": "IP-FAN-100",
    "desc": "Vinik Fan 120 da PCYes, ideal para quem busca desempenho e qualidade em fan.",
    "specs": {
      "Tamanho": "120mm",
      "Quantidade": "1 unidade",
      "Iluminação": "Sem iluminação",
      "Garantia": "6 meses"
    }
  },
  {
    "id": 101,
    "name": "Kumara K552 RGB",
    "brand": "Redragon",
    "category": "Teclado",
    "price": 349.9,
    "salePrice": 279.9,
    "stock": 35,
    "rating": 4.7,
    "reviews": 210,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-KBD-101",
    "desc": "Kumara K552 RGB da Redragon, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Blue",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 102,
    "name": "Mecânico Branco RGB",
    "brand": "Redragon",
    "category": "Teclado",
    "price": 399.9,
    "salePrice": 329.9,
    "stock": 18,
    "rating": 4.8,
    "reviews": 42,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-KBD-102",
    "desc": "Mecânico Branco RGB da Redragon, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Red",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 103,
    "name": "G213 Prodigy",
    "brand": "Logitech",
    "category": "Teclado",
    "price": 299.9,
    "salePrice": 249.9,
    "stock": 24,
    "rating": 4.6,
    "reviews": 88,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-103",
    "desc": "G213 Prodigy da Logitech, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Membrana",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 104,
    "name": "Alloy Origins",
    "brand": "HyperX",
    "category": "Teclado",
    "price": 599.9,
    "salePrice": 499.9,
    "stock": 10,
    "rating": 4.8,
    "reviews": 30,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-104",
    "desc": "Alloy Origins da HyperX, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Aqua",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 105,
    "name": "Shrapnel K592 RGB",
    "brand": "Redragon",
    "category": "Teclado",
    "price": 449.9,
    "salePrice": 379.9,
    "stock": 15,
    "rating": 4.6,
    "reviews": 26,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-105",
    "desc": "Shrapnel K592 RGB da Redragon, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Brown",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 106,
    "name": "Huntsman Mini",
    "brand": "Razer",
    "category": "Teclado",
    "price": 899.9,
    "salePrice": 799.9,
    "stock": 6,
    "rating": 4.9,
    "reviews": 12,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-KBD-106",
    "desc": "Huntsman Mini da Razer, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Óptico",
      "Layout": "ANSI 60%",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 107,
    "name": "K70 RGB",
    "brand": "Corsair",
    "category": "Teclado",
    "price": 999.9,
    "salePrice": 879.9,
    "stock": 5,
    "rating": 4.9,
    "reviews": 15,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-107",
    "desc": "K70 RGB da Corsair, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Cherry MX Red",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 108,
    "name": "GK-70",
    "brand": "Fortrek",
    "category": "Teclado",
    "price": 189.9,
    "salePrice": 149.9,
    "stock": 28,
    "rating": 4.3,
    "reviews": 55,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-108",
    "desc": "GK-70 da Fortrek, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Blue",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 109,
    "name": "Squad RGB",
    "brand": "Pichau",
    "category": "Teclado",
    "price": 229.9,
    "salePrice": 179.9,
    "stock": 22,
    "rating": 4.4,
    "reviews": 34,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-109",
    "desc": "Squad RGB da Pichau, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Red",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 110,
    "name": "K500",
    "brand": "Machenike",
    "category": "Teclado",
    "price": 259.9,
    "salePrice": 209.9,
    "stock": 19,
    "rating": 4.5,
    "reviews": 21,
    "image": "",
    "badges": [],
    "sku": "IP-KBD-110",
    "desc": "K500 da Machenike, ideal para quem busca desempenho e qualidade em teclado.",
    "specs": {
      "Switch": "Blue",
      "Layout": "ABNT2",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 111,
    "name": "Cobra M711",
    "brand": "Redragon",
    "category": "Mouse",
    "price": 199.9,
    "salePrice": 149.9,
    "stock": 50,
    "rating": 4.6,
    "reviews": 178,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-111",
    "desc": "Cobra M711 da Redragon, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "10000",
      "Botões": "7",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 112,
    "name": "G203 Lightsync",
    "brand": "Logitech",
    "category": "Mouse",
    "price": 149.9,
    "salePrice": 119.9,
    "stock": 45,
    "rating": 4.7,
    "reviews": 210,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-MOU-112",
    "desc": "G203 Lightsync da Logitech, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "8000",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 113,
    "name": "DeathAdder Essential",
    "brand": "Razer",
    "category": "Mouse",
    "price": 179.9,
    "salePrice": 139.9,
    "stock": 30,
    "rating": 4.7,
    "reviews": 95,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-113",
    "desc": "DeathAdder Essential da Razer, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "6400",
      "Botões": "5",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 114,
    "name": "Pulsefire Core",
    "brand": "HyperX",
    "category": "Mouse",
    "price": 159.9,
    "salePrice": 129.9,
    "stock": 32,
    "rating": 4.6,
    "reviews": 62,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-114",
    "desc": "Pulsefire Core da HyperX, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "6200",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 115,
    "name": "Vickers",
    "brand": "Fortrek",
    "category": "Mouse",
    "price": 89.9,
    "salePrice": 69.9,
    "stock": 55,
    "rating": 4.3,
    "reviews": 88,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-115",
    "desc": "Vickers da Fortrek, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "3200",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 116,
    "name": "Nyx",
    "brand": "Pichau",
    "category": "Mouse",
    "price": 99.9,
    "salePrice": 79.9,
    "stock": 48,
    "rating": 4.4,
    "reviews": 40,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-116",
    "desc": "Nyx da Pichau, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "7200",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 117,
    "name": "Harpoon RGB",
    "brand": "Corsair",
    "category": "Mouse",
    "price": 199.9,
    "salePrice": 159.9,
    "stock": 25,
    "rating": 4.7,
    "reviews": 51,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-MOU-117",
    "desc": "Harpoon RGB da Corsair, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "6000",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 118,
    "name": "Primer",
    "brand": "XPG",
    "category": "Mouse",
    "price": 139.9,
    "salePrice": 109.9,
    "stock": 34,
    "rating": 4.5,
    "reviews": 29,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-118",
    "desc": "Primer da XPG, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "12000",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 119,
    "name": "Griffin",
    "brand": "Redragon",
    "category": "Mouse",
    "price": 119.9,
    "salePrice": 89.9,
    "stock": 42,
    "rating": 4.4,
    "reviews": 66,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-119",
    "desc": "Griffin da Redragon, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "7200",
      "Botões": "7",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 120,
    "name": "M711",
    "brand": "Machenike",
    "category": "Mouse",
    "price": 79.9,
    "salePrice": 59.9,
    "stock": 60,
    "rating": 4.2,
    "reviews": 37,
    "image": "",
    "badges": [],
    "sku": "IP-MOU-120",
    "desc": "M711 da Machenike, ideal para quem busca desempenho e qualidade em mouse.",
    "specs": {
      "DPI": "5000",
      "Botões": "6",
      "Conexão": "USB",
      "Garantia": "6 meses"
    }
  },
  {
    "id": 121,
    "name": "Zeus Pro Branco",
    "brand": "Redragon",
    "category": "Headset",
    "price": 449.9,
    "salePrice": 369.9,
    "stock": 19,
    "rating": 4.7,
    "reviews": 55,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-HST-121",
    "desc": "Zeus Pro Branco da Redragon, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "7.1 Virtual",
      "Drivers": "50mm",
      "Conexão": "USB/Bluetooth",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 122,
    "name": "Cloud Stinger",
    "brand": "HyperX",
    "category": "Headset",
    "price": 349.9,
    "salePrice": 289.9,
    "stock": 24,
    "rating": 4.8,
    "reviews": 140,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-HST-122",
    "desc": "Cloud Stinger da HyperX, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "50mm",
      "Conexão": "P2",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 123,
    "name": "G435",
    "brand": "Logitech",
    "category": "Headset",
    "price": 399.9,
    "salePrice": 329.9,
    "stock": 18,
    "rating": 4.7,
    "reviews": 40,
    "image": "",
    "badges": [],
    "sku": "IP-HST-123",
    "desc": "G435 da Logitech, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "40mm",
      "Conexão": "Bluetooth/USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 124,
    "name": "Kraken X",
    "brand": "Razer",
    "category": "Headset",
    "price": 499.9,
    "salePrice": 419.9,
    "stock": 12,
    "rating": 4.7,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-HST-124",
    "desc": "Kraken X da Razer, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "7.1 Virtual",
      "Drivers": "50mm",
      "Conexão": "USB/P2",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 125,
    "name": "H1",
    "brand": "Fortrek",
    "category": "Headset",
    "price": 129.9,
    "salePrice": 99.9,
    "stock": 40,
    "rating": 4.3,
    "reviews": 78,
    "image": "",
    "badges": [],
    "sku": "IP-HST-125",
    "desc": "H1 da Fortrek, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "40mm",
      "Conexão": "P2",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 126,
    "name": "HS55",
    "brand": "Corsair",
    "category": "Headset",
    "price": 399.9,
    "salePrice": 339.9,
    "stock": 14,
    "rating": 4.7,
    "reviews": 25,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-HST-126",
    "desc": "HS55 da Corsair, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "50mm",
      "Conexão": "P2",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 127,
    "name": "Kripton",
    "brand": "Pichau",
    "category": "Headset",
    "price": 179.9,
    "salePrice": 139.9,
    "stock": 30,
    "rating": 4.4,
    "reviews": 52,
    "image": "",
    "badges": [],
    "sku": "IP-HST-127",
    "desc": "Kripton da Pichau, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "7.1 Virtual",
      "Drivers": "50mm",
      "Conexão": "USB",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 128,
    "name": "Quantum 100",
    "brand": "JBL",
    "category": "Headset",
    "price": 249.9,
    "salePrice": 199.9,
    "stock": 20,
    "rating": 4.5,
    "reviews": 36,
    "image": "",
    "badges": [],
    "sku": "IP-HST-128",
    "desc": "Quantum 100 da JBL, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "40mm",
      "Conexão": "P2",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 129,
    "name": "Ares H120",
    "brand": "Redragon",
    "category": "Headset",
    "price": 149.9,
    "salePrice": 119.9,
    "stock": 35,
    "rating": 4.3,
    "reviews": 63,
    "image": "",
    "badges": [],
    "sku": "IP-HST-129",
    "desc": "Ares H120 da Redragon, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "Estéreo",
      "Drivers": "40mm",
      "Conexão": "P2",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 130,
    "name": "Emix H30",
    "brand": "XPG",
    "category": "Headset",
    "price": 299.9,
    "salePrice": 249.9,
    "stock": 16,
    "rating": 4.6,
    "reviews": 18,
    "image": "",
    "badges": [],
    "sku": "IP-HST-130",
    "desc": "Emix H30 da XPG, ideal para quem busca desempenho e qualidade em headset.",
    "specs": {
      "Som": "7.1 Virtual",
      "Drivers": "50mm",
      "Conexão": "USB",
      "Garantia": "24 meses"
    }
  },
  {
    "id": 131,
    "name": "Gaming 24'' Full HD 165Hz",
    "brand": "Philco",
    "category": "Monitor",
    "price": 999.9,
    "salePrice": 849.9,
    "stock": 13,
    "rating": 4.7,
    "reviews": 31,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-MON-131",
    "desc": "Gaming 24'' Full HD 165Hz da Philco, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "24\"",
      "Resolução": "1920x1080",
      "Taxa": "165Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 132,
    "name": "UltraGear 27'' 4K 144Hz",
    "brand": "LG",
    "category": "Monitor",
    "price": 3299.9,
    "salePrice": 2899.9,
    "stock": 6,
    "rating": 4.9,
    "reviews": 22,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-MON-132",
    "desc": "UltraGear 27'' 4K 144Hz da LG, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "27\"",
      "Resolução": "3840x2160",
      "Taxa": "144Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 133,
    "name": "24G2 24'' Full HD 144Hz",
    "brand": "AOC",
    "category": "Monitor",
    "price": 1099.9,
    "salePrice": 949.9,
    "stock": 12,
    "rating": 4.7,
    "reviews": 44,
    "image": "",
    "badges": [],
    "sku": "IP-MON-133",
    "desc": "24G2 24'' Full HD 144Hz da AOC, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "24\"",
      "Resolução": "1920x1080",
      "Taxa": "144Hz",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 134,
    "name": "Odyssey G5 27'' QHD 165Hz",
    "brand": "Samsung",
    "category": "Monitor",
    "price": 1899.9,
    "salePrice": 1649.9,
    "stock": 8,
    "rating": 4.8,
    "reviews": 27,
    "image": "",
    "badges": [],
    "sku": "IP-MON-134",
    "desc": "Odyssey G5 27'' QHD 165Hz da Samsung, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "27\"",
      "Resolução": "2560x1440",
      "Taxa": "165Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 135,
    "name": "24GN600 24'' Full HD 144Hz",
    "brand": "LG",
    "category": "Monitor",
    "price": 1199.9,
    "salePrice": 999.9,
    "stock": 10,
    "rating": 4.7,
    "reviews": 35,
    "image": "",
    "badges": [],
    "sku": "IP-MON-135",
    "desc": "24GN600 24'' Full HD 144Hz da LG, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "24\"",
      "Resolução": "1920x1080",
      "Taxa": "144Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 136,
    "name": "G27F 27'' Full HD 165Hz",
    "brand": "Gigabyte",
    "category": "Monitor",
    "price": 1399.9,
    "salePrice": 1199.9,
    "stock": 9,
    "rating": 4.7,
    "reviews": 20,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-MON-136",
    "desc": "G27F 27'' Full HD 165Hz da Gigabyte, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "27\"",
      "Resolução": "1920x1080",
      "Taxa": "165Hz",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 137,
    "name": "VX2418 24'' Full HD 75Hz",
    "brand": "ViewSonic",
    "category": "Monitor",
    "price": 699.9,
    "salePrice": 599.9,
    "stock": 18,
    "rating": 4.4,
    "reviews": 48,
    "image": "",
    "badges": [],
    "sku": "IP-MON-137",
    "desc": "VX2418 24'' Full HD 75Hz da ViewSonic, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "24\"",
      "Resolução": "1920x1080",
      "Taxa": "75Hz",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 138,
    "name": "S2721HGF 27'' Full HD 144Hz",
    "brand": "Dell",
    "category": "Monitor",
    "price": 1499.9,
    "salePrice": 1299.9,
    "stock": 7,
    "rating": 4.8,
    "reviews": 16,
    "image": "",
    "badges": [],
    "sku": "IP-MON-138",
    "desc": "S2721HGF 27'' Full HD 144Hz da Dell, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "27\"",
      "Resolução": "1920x1080",
      "Taxa": "144Hz",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 139,
    "name": "Hero 24.5'' Full HD 165Hz",
    "brand": "AOC",
    "category": "Monitor",
    "price": 1199.9,
    "salePrice": 999.9,
    "stock": 11,
    "rating": 4.6,
    "reviews": 24,
    "image": "",
    "badges": [],
    "sku": "IP-MON-139",
    "desc": "Hero 24.5'' Full HD 165Hz da AOC, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "24.5\"",
      "Resolução": "1920x1080",
      "Taxa": "165Hz",
      "Garantia": "36 meses"
    }
  },
  {
    "id": 140,
    "name": "21.5'' Full HD 75Hz",
    "brand": "Positivo",
    "category": "Monitor",
    "price": 599.9,
    "salePrice": 499.9,
    "stock": 20,
    "rating": 4.2,
    "reviews": 60,
    "image": "",
    "badges": [],
    "sku": "IP-MON-140",
    "desc": "21.5'' Full HD 75Hz da Positivo, ideal para quem busca desempenho e qualidade em monitor.",
    "specs": {
      "Tamanho": "21.5\"",
      "Resolução": "1920x1080",
      "Taxa": "75Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 141,
    "name": "Cabo de Força PC",
    "brand": "Corsair",
    "category": "Cabos e Adaptadores",
    "price": 39.9,
    "salePrice": 29.9,
    "stock": 100,
    "rating": 4.4,
    "reviews": 22,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-141",
    "desc": "Cabo de Força PC da Corsair, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "1.5m",
      "Padrão": "ABNT NBR 14136",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 142,
    "name": "Hub USB 3.0 4 Portas",
    "brand": "MSI",
    "category": "Cabos e Adaptadores",
    "price": 89.9,
    "salePrice": 69.9,
    "stock": 45,
    "rating": 4.3,
    "reviews": 17,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-142",
    "desc": "Hub USB 3.0 4 Portas da MSI, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Portas": "4x USB 3.0",
      "Velocidade": "5Gbps",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 143,
    "name": "Cabo HDMI 2.1 2m",
    "brand": "Redragon",
    "category": "Cabos e Adaptadores",
    "price": 59.9,
    "salePrice": 44.9,
    "stock": 60,
    "rating": 4.5,
    "reviews": 38,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-143",
    "desc": "Cabo HDMI 2.1 2m da Redragon, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "2m",
      "Padrão": "HDMI 2.1",
      "Resolução Máx": "4K 120Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 144,
    "name": "Cabo SATA III 50cm",
    "brand": "Genérico",
    "category": "Cabos e Adaptadores",
    "price": 14.9,
    "salePrice": 9.9,
    "stock": 200,
    "rating": 4.2,
    "reviews": 65,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-144",
    "desc": "Cabo SATA III 50cm da Genérico, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "50cm",
      "Padrão": "SATA III",
      "Velocidade": "6Gbps",
      "Garantia": "6 meses"
    }
  },
  {
    "id": 145,
    "name": "Adaptador USB-C para HDMI",
    "brand": "XPG",
    "category": "Cabos e Adaptadores",
    "price": 79.9,
    "salePrice": 59.9,
    "stock": 50,
    "rating": 4.4,
    "reviews": 29,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-CBL-145",
    "desc": "Adaptador USB-C para HDMI da XPG, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Entrada": "USB-C",
      "Saída": "HDMI",
      "Resolução Máx": "4K 60Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 146,
    "name": "Cabo DisplayPort 1.4 1.5m",
    "brand": "Corsair",
    "category": "Cabos e Adaptadores",
    "price": 69.9,
    "salePrice": 54.9,
    "stock": 40,
    "rating": 4.6,
    "reviews": 15,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-146",
    "desc": "Cabo DisplayPort 1.4 1.5m da Corsair, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "1.5m",
      "Padrão": "DisplayPort 1.4",
      "Resolução Máx": "8K 60Hz",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 147,
    "name": "Extensor PCIe 8-pin",
    "brand": "Pichau",
    "category": "Cabos e Adaptadores",
    "price": 24.9,
    "salePrice": 17.9,
    "stock": 90,
    "rating": 4.3,
    "reviews": 12,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-147",
    "desc": "Extensor PCIe 8-pin da Pichau, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "30cm",
      "Tipo": "Extensor PCIe 8-pin",
      "Garantia": "6 meses"
    }
  },
  {
    "id": 148,
    "name": "Cabo de Rede Cat6 3m",
    "brand": "Redragon",
    "category": "Cabos e Adaptadores",
    "price": 29.9,
    "salePrice": 21.9,
    "stock": 120,
    "rating": 4.5,
    "reviews": 44,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-148",
    "desc": "Cabo de Rede Cat6 3m da Redragon, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Comprimento": "3m",
      "Padrão": "Cat6",
      "Velocidade": "1Gbps",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 149,
    "name": "Adaptador M.2 para USB 3.0",
    "brand": "XPG",
    "category": "Cabos e Adaptadores",
    "price": 99.9,
    "salePrice": 79.9,
    "stock": 30,
    "rating": 4.5,
    "reviews": 20,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-149",
    "desc": "Adaptador M.2 para USB 3.0 da XPG, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Entrada": "M.2 NVMe/SATA",
      "Saída": "USB 3.1",
      "Velocidade": "10Gbps",
      "Garantia": "12 meses"
    }
  },
  {
    "id": 150,
    "name": "Organizador de Cabos Velcro (Kit)",
    "brand": "Genérico",
    "category": "Cabos e Adaptadores",
    "price": 19.9,
    "salePrice": 12.9,
    "stock": 150,
    "rating": 4.4,
    "reviews": 33,
    "image": "",
    "badges": [],
    "sku": "IP-CBL-150",
    "desc": "Organizador de Cabos Velcro (Kit) da Genérico, ideal para quem busca desempenho e qualidade em cabos e adaptadores.",
    "specs": {
      "Quantidade": "20 unidades",
      "Material": "Velcro",
      "Garantia": "Não aplicável"
    }
  },
  {
    "id": 151,
    "name": "Infinity Starter — Ryzen 5 5600 / RTX 3060 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 6999.9,
    "salePrice": 6299.9,
    "stock": 5,
    "rating": 4.8,
    "reviews": 14,
    "image": "",
    "badges": [
      "MAIS VENDIDO"
    ],
    "sku": "IP-PCM-151",
    "desc": "PC gamer completo, montado e testado pela equipe Infinity Parts, pronto para jogar em Full HD com boa taxa de quadros.",
    "specs": {
      "Processador": "AMD Ryzen 5 5600",
      "Placa de Vídeo": "RTX 3060 12GB",
      "Memória": "16GB DDR4 3200MHz",
      "Armazenamento": "SSD NVMe 500GB",
      "Fonte": "550W 80 Plus Bronze",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 152,
    "name": "Infinity Racer — Ryzen 5 7600 / RTX 4060 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 8999.9,
    "salePrice": 7999.9,
    "stock": 4,
    "rating": 4.8,
    "reviews": 11,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-PCM-152",
    "desc": "Infinity Racer — Ryzen 5 7600 / RTX 4060 / 16GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 5 7600",
      "Placa de Vídeo": "RTX 4060 8GB",
      "Memória": "16GB DDR5 5600MHz",
      "Armazenamento": "SSD NVMe 1TB",
      "Fonte": "650W 80 Plus Bronze",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 153,
    "name": "Infinity Strike — Ryzen 7 7700X / RTX 4070 / 32GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 13999.9,
    "salePrice": 12499.9,
    "stock": 3,
    "rating": 4.9,
    "reviews": 8,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-PCM-153",
    "desc": "Infinity Strike — Ryzen 7 7700X / RTX 4070 / 32GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 7 7700X",
      "Placa de Vídeo": "RTX 4070 12GB",
      "Memória": "32GB DDR5 6000MHz",
      "Armazenamento": "SSD NVMe 1TB",
      "Fonte": "750W 80 Plus Gold",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 154,
    "name": "Infinity Core — Core i5-12400F / RTX 3060 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 7499.9,
    "salePrice": 6699.9,
    "stock": 6,
    "rating": 4.7,
    "reviews": 17,
    "image": "",
    "badges": [],
    "sku": "IP-PCM-154",
    "desc": "Infinity Core — Core i5-12400F / RTX 3060 / 16GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "Intel Core i5-12400F",
      "Placa de Vídeo": "RTX 3060 12GB",
      "Memória": "16GB DDR4 3200MHz",
      "Armazenamento": "SSD NVMe 500GB",
      "Fonte": "550W 80 Plus Bronze",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 155,
    "name": "Infinity Ultra — Core i7-13700K / RTX 4070 Ti Super / 32GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 17999.9,
    "salePrice": 15999.9,
    "stock": 2,
    "rating": 4.9,
    "reviews": 5,
    "image": "",
    "badges": [
      "ÚLTIMAS UNIDADES"
    ],
    "sku": "IP-PCM-155",
    "desc": "Infinity Ultra — Core i7-13700K / RTX 4070 Ti Super / 32GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "Intel Core i7-13700K",
      "Placa de Vídeo": "RTX 4070 Ti Super 16GB",
      "Memória": "32GB DDR5 6000MHz",
      "Armazenamento": "SSD NVMe 2TB",
      "Fonte": "850W 80 Plus Gold",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 156,
    "name": "Infinity Extreme — Ryzen 9 7900X / RTX 5090 / 64GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 29999.9,
    "salePrice": 27999.9,
    "stock": 1,
    "rating": 5.0,
    "reviews": 3,
    "image": "",
    "badges": [
      "NOVO"
    ],
    "sku": "IP-PCM-156",
    "desc": "Infinity Extreme — Ryzen 9 7900X / RTX 5090 / 64GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 9 7900X",
      "Placa de Vídeo": "RTX 5090 32GB",
      "Memória": "64GB DDR5 6000MHz",
      "Armazenamento": "SSD NVMe 2TB",
      "Fonte": "1000W 80 Plus Gold",
      "Gabinete": "Full Tower Vidro Temperado"
    }
  },
  {
    "id": 157,
    "name": "Infinity Office+ — Ryzen 5 5600 / GT 1030 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 4499.9,
    "salePrice": 3999.9,
    "stock": 10,
    "rating": 4.5,
    "reviews": 20,
    "image": "",
    "badges": [],
    "sku": "IP-PCM-157",
    "desc": "Infinity Office+ — Ryzen 5 5600 / GT 1030 / 16GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 5 5600",
      "Placa de Vídeo": "GT 1030 2GB",
      "Memória": "16GB DDR4 3200MHz",
      "Armazenamento": "SSD 480GB",
      "Fonte": "450W",
      "Gabinete": "Mid Tower Compacto"
    }
  },
  {
    "id": 158,
    "name": "Infinity Stream — Ryzen 7 5700X / RX 7700 XT / 32GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 10999.9,
    "salePrice": 9799.9,
    "stock": 4,
    "rating": 4.8,
    "reviews": 9,
    "image": "",
    "badges": [],
    "sku": "IP-PCM-158",
    "desc": "Infinity Stream — Ryzen 7 5700X / RX 7700 XT / 32GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 7 5700X",
      "Placa de Vídeo": "RX 7700 XT 12GB",
      "Memória": "32GB DDR4 3200MHz",
      "Armazenamento": "SSD NVMe 1TB",
      "Fonte": "650W 80 Plus Bronze",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  },
  {
    "id": 159,
    "name": "Infinity Compact — Core i5-13400F / RTX 4060 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 8499.9,
    "salePrice": 7599.9,
    "stock": 5,
    "rating": 4.7,
    "reviews": 12,
    "image": "",
    "badges": [],
    "sku": "IP-PCM-159",
    "desc": "Infinity Compact — Core i5-13400F / RTX 4060 / 16GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "Intel Core i5-13400F",
      "Placa de Vídeo": "RTX 4060 8GB",
      "Memória": "16GB DDR4 3200MHz",
      "Armazenamento": "SSD NVMe 1TB",
      "Fonte": "600W 80 Plus Bronze",
      "Gabinete": "Mid Tower Compacto"
    }
  },
  {
    "id": 160,
    "name": "Infinity Creator — Ryzen 9 7900X / RTX 4070 Ti Super / 64GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 19999.9,
    "salePrice": 17999.9,
    "stock": 2,
    "rating": 4.9,
    "reviews": 6,
    "image": "",
    "badges": [],
    "sku": "IP-PCM-160",
    "desc": "Infinity Creator — Ryzen 9 7900X / RTX 4070 Ti Super / 64GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 9 7900X",
      "Placa de Vídeo": "RTX 4070 Ti Super 16GB",
      "Memória": "64GB DDR5 6000MHz",
      "Armazenamento": "SSD NVMe 2TB",
      "Fonte": "850W 80 Plus Gold",
      "Gabinete": "Full Tower Vidro Temperado"
    }
  },
  {
    "id": 161,
    "name": "Infinity Entry — Ryzen 5 5600 / RX 7600 / 16GB",
    "brand": "Infinity Parts",
    "category": "PC Montado",
    "price": 6499.9,
    "salePrice": 5799.9,
    "stock": 7,
    "rating": 4.6,
    "reviews": 15,
    "image": "",
    "badges": [
      "OFERTA"
    ],
    "sku": "IP-PCM-161",
    "desc": "Infinity Entry — Ryzen 5 5600 / RX 7600 / 16GB da Infinity Parts, ideal para quem busca desempenho e qualidade em pc montado.",
    "specs": {
      "Processador": "AMD Ryzen 5 5600",
      "Placa de Vídeo": "RX 7600 8GB",
      "Memória": "16GB DDR4 3200MHz",
      "Armazenamento": "SSD NVMe 500GB",
      "Fonte": "550W 80 Plus Bronze",
      "Gabinete": "Mid Tower Vidro Temperado"
    }
  }
];

/* ============================================================
   CONEXÃO COM A API (backend próprio / app desktop)
   ============================================================
   Troque apenas a linha "baseUrl" abaixo pelo endereço real da
   sua API (ex: "https://localhost:7080/api" ou a URL publicada).

   Endpoint esperado: GET {baseUrl}{productsPath}
   Deve retornar um array JSON, onde cada item segue este formato
   (mesmo shape usado nos dados de demonstração acima):

   {
     "id": 1,
     "name": "RTX 4060 8GB",
     "brand": "NVIDIA",
     "category": "Placa de Vídeo",
     "price": 2499.90,        // preço "de"
     "salePrice": 2199.90,    // preço "por" (o site calcula Pix e parcelas sozinho)
     "stock": 8,
     "rating": 4.8,
     "reviews": 37,
     "image": "https://.../foto.jpg",   // URL completa OU caminho tipo "assets/products/x.svg"
     "badges": ["OFERTA"],              // pode vir [] ou omitido
     "sku": "IP-GPU-001",
     "desc": "Descrição do produto.",
     "specs": { "Memória": "8GB GDDR6", "Interface": "PCI Express 4.0" }
   }

   Se a API estiver fora do ar, não responder nesse formato, ou
   "useApi" estiver como false, o site automaticamente volta a
   usar o catálogo de demonstração (FALLBACK_PRODUCTS) acima, sem
   quebrar nada — dá pra continuar trabalhando no site mesmo sem
   a API rodando no momento.
   ============================================================ */
const API_CONFIG = {
  useApi: true,
  baseUrl: "http://localhost:5022/api",
  productsPath: "/Produto",
  productPath: id => `/Produto/${id}`,
  timeoutMs: 10000
};

let PRODUCTS = [];
let usedFallbackProducts = false;

/* Resolve o caminho da imagem tanto para URL completa vinda da API
   quanto para os caminhos relativos que venham a ser usados no futuro. */
function resolveImageUrl(image) {
  if (!image) return "";
  if (/^https?:\/\//i.test(image) || image.startsWith("/")) return image;
  return image; // caminho relativo funciona direto
}

/* Gera a imagem do produto se ela existir (vinda da API/banco), ou um
   placeholder discreto "sem imagem" enquanto o cadastro de produtos
   (feito pelo app desktop) ainda não tiver uma foto associada. */
function productImageHtml(product, linkHref) {
  const inner = product.image
    ? `<img src="${resolveImageUrl(product.image)}" alt="${product.name}" onerror="this.parentElement.innerHTML='<div class=&quot;no-image-placeholder&quot;><i class=&quot;bi bi-image&quot;></i><span>Sem imagem</span></div>'">`
    : `<div class="no-image-placeholder"><i class="bi bi-image"></i><span>Sem imagem</span></div>`;
  return `<a href="${linkHref}">${inner}</a>`;
}

async function fetchProductsFromApi() {
  const controller = new AbortController();

  const timeout = setTimeout(() => {
    controller.abort();
  }, API_CONFIG.timeoutMs);

  try {
    const response = await fetch(
      API_CONFIG.baseUrl + API_CONFIG.productsPath,
      {
        method: "GET",
        headers: {
          "Accept": "application/json"
        },
        signal: controller.signal
      }
    );

    clearTimeout(timeout);

    if (!response.ok) {
      throw new Error(
        `Erro na API: ${response.status} ${response.statusText}`
      );
    }

    const data = await response.json();

    if (!Array.isArray(data)) {
      throw new Error("A API não retornou uma lista de produtos.");
    }

    /*
     * Converte o ProdutoDto do C#
 para o formato que a UI atual já utiliza.
     */
    return data.map(produto => ({
  id: produto.id,
  name: produto.nome,
  brand: `Marca #${produto.marcaId}`,
  category: "Produto",
  price: Number(produto.preco),
  salePrice: Number(produto.preco),
  stock: Number(produto.quantidadeEstoque),
  rating: 0,
  reviews: 0,
  image: produto.image || "",
  badges: [],
  sku: produto.codigo,
  desc: produto.descricao || "Sem descrição disponível.",
  specs: {
    Código: produto.codigo,
    Estoque: `${produto.quantidadeEstoque} unidades`,
    "ID da marca": produto.marcaId
  },

  apiData: produto
}));

  } catch (error) {
    clearTimeout(timeout);

    console.error(
      "[Infinity Parts] Erro ao carregar produtos da API:",
      error
    );

    throw error;
  }
}


/* Carrega os produtos: tenta a API primeiro (se habilitada), e cai para o
   catálogo de demonstração em caso de falha. Todo o resto do site (main.js,
   filters.js, cart.js, checkout.js, account.js, produto.html) espera essa
   Promise terminar antes de renderizar qualquer coisa que dependa de PRODUCTS. */
async function initProducts() {
  if (API_CONFIG.useApi) {
    try {
      PRODUCTS = await fetchProductsFromApi();
      usedFallbackProducts = false;
      console.info(`[Infinity Parts] ${PRODUCTS.length} produtos carregados da API (${API_CONFIG.baseUrl}${API_CONFIG.productsPath}).`);
      return;
    } catch (err) {
      console.warn("[Infinity Parts] Não foi possível carregar produtos da API, usando catálogo de demonstração.", err);
    }
  }
  PRODUCTS = FALLBACK_PRODUCTS;
  usedFallbackProducts = true;
}

/* Inicia o carregamento assim que este arquivo é lido (em paralelo ao
   parse do restante da página) — os outros scripts fazem
   `await window.productsReadyPromise` antes de usar PRODUCTS. */
window.productsReadyPromise = initProducts();

/* Calcula preços derivados: desconto %, preço Pix (5% off) e parcelamento (10x sem juros) */
function computePricing(product) {
  const discount = Math.round(((product.price - product.salePrice) / product.price) * 100);
  const pixPrice = +(product.salePrice * 0.95).toFixed(2);
  const installments = 10;
  const installmentValue = +(product.salePrice / installments).toFixed(2);
  return { discount, pixPrice, installments, installmentValue };
}

function formatBRL(value) {
  return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

function getStockLabel(stock) {
  if (stock <= 0) return { text: "Esgotado", class: "stock-out" };
  if (stock <= 3) return { text: `Últimas ${stock} unidades`, class: "stock-low" };
  return { text: "Em estoque", class: "stock-ok" };
}

function renderStars(rating) {
  const full = Math.round(rating);
  let html = '<span class="stars" aria-label="' + rating + ' de 5 estrelas">';
  for (let i = 1; i <= 5; i++) {
    html += i <= full ? '<i class="bi bi-star-fill"></i>' : '<i class="bi bi-star"></i>';
  }
  html += '</span>';
  return html;
}

/* Gera o HTML de um card de produto (usado na home, catálogo, relacionados e favoritos) */
function renderProductCard(product) {
  const { discount, pixPrice, installments, installmentValue } = computePricing(product);
  const stock = getStockLabel(product.stock);
  const isFav = isFavorite(product.id);
  const badges = [...product.badges];
  if (discount > 0 && !badges.includes("OFERTA") && product.stock > 0) badges.unshift("OFERTA");

  const badgeHtml = badges.map(b => `<span class="badge-tag badge-${slugify(b)}">${b}</span>`).join('');
  const buyDisabled = product.stock <= 0;

  return `
  <div class="product-card" data-id="${product.id}" data-category="${product.category}" data-brand="${product.brand}" data-price="${product.salePrice}">
    <div class="product-card-media">
      ${badgeHtml ? `<div class="product-badges">${badgeHtml}</div>` : ''}
      <button class="btn-fav ${isFav ? 'active' : ''}" data-id="${product.id}" aria-label="Favoritar produto" title="Favoritar">
        <i class="bi ${isFav ? 'bi-heart-fill' : 'bi-heart'}"></i>
      </button>
      <a href="produto.html?id=${product.id}">
        ${product.image
          ? `<img src="${resolveImageUrl(product.image)}" alt="${product.name}" onerror="this.parentElement.innerHTML='<div class=&quot;no-image-placeholder&quot;><i class=&quot;bi bi-image&quot;></i><span>Sem imagem</span></div>'">`
          : `<div class="no-image-placeholder"><i class="bi bi-image"></i><span>Sem imagem</span></div>`}
      </a>
    </div>
    <div class="product-card-body">
      <span class="product-brand">${product.brand}</span>
      <a href="produto.html?id=${product.id}" class="product-name">${product.name}</a>
      <div class="product-rating">${renderStars(product.rating)} <span class="reviews-count">(${product.reviews})</span></div>
      <div class="product-price-box">
        ${discount > 0 ? `<span class="old-price">${formatBRL(product.price)}</span>` : ''}
        <span class="current-price">${formatBRL(product.salePrice)}</span>
        <span class="pix-price"><i class="bi bi-pix"></i> ${formatBRL(pixPrice)} no Pix</span>
        <span class="installments">ou ${installments}x de ${formatBRL(installmentValue)} sem juros</span>
      </div>
      <span class="stock-label ${stock.class}"><i class="bi bi-check-circle"></i> ${stock.text}</span>
      <div class="product-card-actions">
        <button class="btn btn-primary btn-buy" data-id="${product.id}" ${buyDisabled ? 'disabled' : ''}>
          <i class="bi bi-cart-plus"></i> ${buyDisabled ? 'AVISE-ME' : 'COMPRAR'}
        </button>
      </div>
    </div>
  </div>`;
}

/* Gera o HTML de um card do carrossel "PCs Montados" (imagem + specs resumidas + preço) */
function renderPcCard(product) {
  const { discount, pixPrice, installments, installmentValue } = computePricing(product);
  const stock = getStockLabel(product.stock);
  const specsLine = [product.specs["Processador"], product.specs["Placa de Vídeo"], product.specs["Memória"]].filter(Boolean).join(' • ');
  const badges = [...product.badges];
  const badgeHtml = badges.map(b => `<span class="badge-tag badge-${slugify(b)}">${b}</span>`).join('');
  const buyDisabled = product.stock <= 0;

  return `
  <div class="pc-card" data-id="${product.id}">
    <div class="pc-card-media">
      ${badgeHtml ? `<div class="product-badges">${badgeHtml}</div>` : ''}
      <a href="produto.html?id=${product.id}">${product.image
          ? `<img src="${resolveImageUrl(product.image)}" alt="${product.name}" onerror="this.parentElement.innerHTML='<div class=&quot;no-image-placeholder&quot;><i class=&quot;bi bi-image&quot;></i><span>Sem imagem</span></div>'">`
          : `<div class="no-image-placeholder"><i class="bi bi-image"></i><span>Sem imagem</span></div>`}</a>
    </div>
    <div class="pc-card-body">
      <a href="produto.html?id=${product.id}" class="pc-card-name">${product.name}</a>
      <span class="pc-card-specs">${specsLine}</span>
      <div class="product-price-box">
        ${discount > 0 ? `<span class="old-price">${formatBRL(product.price)}</span>` : ''}
        <span class="current-price">${formatBRL(product.salePrice)}</span>
        <span class="pix-price"><i class="bi bi-pix"></i> ${formatBRL(pixPrice)} no Pix</span>
        <span class="installments">ou ${installments}x de ${formatBRL(installmentValue)} sem juros</span>
      </div>
      <span class="stock-label ${stock.class}"><i class="bi bi-check-circle"></i> ${stock.text}</span>
      <button class="btn btn-primary btn-block btn-buy" data-id="${product.id}" ${buyDisabled ? 'disabled' : ''}>
        <i class="bi bi-cart-plus"></i> ${buyDisabled ? 'AVISE-ME' : 'ADICIONAR AO CARRINHO'}
      </button>
    </div>
  </div>`;
}

function slugify(text) {
  return text.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/\s+/g, '-');
}

function getProductById(id) {
  return PRODUCTS.find(p => p.id === Number(id));
}

function getRelatedProducts(product, limit = 4) {
  return PRODUCTS.filter(p => p.category === product.category && p.id !== product.id).slice(0, limit);
}

function searchProducts(term) {
  if (!term) return [];
  const t = term.toLowerCase();
  return PRODUCTS.filter(p =>
    p.name.toLowerCase().includes(t) ||
    p.category.toLowerCase().includes(t) ||
    p.brand.toLowerCase().includes(t) ||
    p.desc.toLowerCase().includes(t)
  );
}
