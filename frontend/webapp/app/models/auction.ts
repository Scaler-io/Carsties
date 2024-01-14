export interface Auction {
  reservePrice: number;
  seller: string;
  winner: any;
  soldAmount: number;
  currentHighBid: number;
  status: string;
  item: AuctionItem;
  auctionEnd: string;
  metadata: Metadata;
  id: string;
}

interface AuctionItem {
  make: string;
  model: string;
  year: number;
  color: string;
  mileage: number;
  imageUrl: string;
}
interface Metadata {
  createdAt: string;
  updatedAt: string;
}
