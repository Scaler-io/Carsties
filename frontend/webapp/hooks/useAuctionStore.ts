import { AuctionSearch } from "@/app/models/auction-serach";
import { PageResult } from "@/app/models/page-result";
import { createWithEqualityFn } from "zustand/traditional";

type State = {
  auctions: AuctionSearch[];
  totalCount: number;
  pageCount: number;
};

type Actions = {
  setData: (data: PageResult<AuctionSearch>) => void;
  setCurrentPrice: (auctionId: string, amount: number) => void;
};

const initialState: State = {
  auctions: [],
  pageCount: 0,
  totalCount: 0,
};

export const useAuctionStore = createWithEqualityFn<State & Actions>((set) => ({
  ...initialState,
  setData: (data: PageResult<AuctionSearch>) => {
    set(() => ({
      auctions: data.data,
      totalCount: data.total,
      pageCount: data.pageCount,
    }));
  },
  setCurrentPrice: (auctionId: string, amount: number) => {
    set((state) => ({
      auctions: state.auctions.map((auction) =>
        auction.id === auctionId
          ? { ...auction, currentHighBid: amount }
          : auction
      ),
    }));
  },
}));
