"use server";
import { Auction } from "../models/auction";
import { PageResult } from "../models/page-result";

export const getAuctions = async (
  pageNumber: number,
  pageSize: number
): Promise<PageResult<Auction>> => {
  const res = await fetch(
    `http://localhost:4003/search?pageSize=${pageSize}&pageNumber=${pageNumber}`
  );
  if (!res.ok) throw new Error("Failed to fetch data");
  return res.json();
};
