import { Auction } from "../models/auction";
import { PageResult } from "../models/page-result";

export const getAuctions = async (
  url: string
): Promise<PageResult<Auction>> => {
  const res = await fetch(`http://localhost:4003/search${url}`);
  if (!res.ok) throw new Error("Failed to fetch data");
  return res.json();
};

export const updateAuction = async (token: string) => {
  const data = {
    milage: Math.floor(Math.random() * 100000) + 1,
  };
  const res = await fetch(
    "http://localhost:4003/auctions/81737de8-1a9e-4183-bfb5-2c08a18dead6",
    {
      method: "PUT",
      headers: {
        "Content-type": "application/json",
        'Authorization': "Bearer " + token,
      },
      body: JSON.stringify(data),
    }
  );

  if (!res.ok) return { status: res.status, message: res.statusText };

  return res;
};
