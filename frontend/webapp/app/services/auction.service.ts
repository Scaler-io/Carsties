"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { Auction } from "../models/auction";
import { PageResult } from "../models/page-result";
import { FieldValues } from "react-hook-form";
import { AuctionSearch } from "../models/auction-serach";
import { revalidatePath } from "next/cache";

export const getAuctions = async (
  url: string
): Promise<PageResult<AuctionSearch>> => {
  return fetchWrapper.get(`search${url}`);
};

export const createAuction = async (data: FieldValues) => {
  return await fetchWrapper.post("auctions", data);
};

export const getAuctionDetailedView = async (id: string): Promise<Auction> => {
  return await fetchWrapper.get(`auctions/${id}`);
};

export const updateAuction = async (data: FieldValues, id: string) => {
  const res = await fetchWrapper.put(`auctions/${id}`, data);
  revalidatePath(`/auctions/${id}`);
  return res;
};
