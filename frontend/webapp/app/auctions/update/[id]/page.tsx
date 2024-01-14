'use client'

import Heading from "@/app/components/heading/heading";
import React, { useEffect, useState } from "react";
import AuctionForm from "../../auction-form";
import { getAuctionDetailedView, updateAuction } from "@/app/services/auction.service";
import { FieldValues } from "react-hook-form";
import { Auction } from "@/app/models/auction";
import Loader from "@/app/components/loader/loader";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

const UpdateAuction = ({ params }: { params: { id: string } }) => {
  const [auction, setAuction] = useState<Auction | null>(null);
  const router = useRouter();

  useEffect(() => {
    const fetchdata = async () => {
      try{
        const data = await getAuctionDetailedView(params.id);
        console.log(data)
        setAuction(data);
      }catch(error){
        console.log(error)
      }
    }
    fetchdata();
  }, [params.id]);

  // handles update auction form action
  const onSubmit = async (formData: FieldValues) => {
    try {
      const res = await updateAuction(formData, params.id);
      if(res){
        toast.success("Auction is updated successfully")
        router.push(`/auctions/details/${params.id}`);
      }
    } catch (error) {
      toast.error(error.status + " " + error.message);
    }
  };

  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading
        title="Update your auction"
        subtitle="Please update the details of your car"
      />
      {auction !== null ? <AuctionForm onNextClick={onSubmit} isUpdateForm auction={auction} /> : <Loader />}
    </div>
  );
};

export default UpdateAuction;
