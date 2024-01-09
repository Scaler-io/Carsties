"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { Auction } from "../models/auction";
import { PageResult } from "../models/page-result";
import { FieldValue, FieldValues } from "react-hook-form";

export const getAuctions = async (
  url: string
): Promise<PageResult<Auction>> => {
  return fetchWrapper.get(`search${url}`);
};

export const updateAuction = async () => {
  const data = {
    make: "BMW",
    model: "GT",
    year: 2022,
    color: "white",
    milage: Math.floor(Math.random() * 100000) + 1,
  };

  return await fetchWrapper.put(
    "auctions/466e4744-4dc5-4987-aae0-b621acfc5e39",
    data
  );
};

export const createAuction = async (data: FieldValues) => {
  return await fetchWrapper.post('auctions', data);
}
