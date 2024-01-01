"use server";
import { Auction } from "../models/auction";
import { PageResult } from "../models/page-result";

export const getAuctions = async (
  url: string
): Promise<PageResult<Auction>> => {
  const res = await fetch(`http://localhost:4003/search${url}`);
  if (!res.ok) throw new Error("Failed to fetch data");
  return res.json();
};
