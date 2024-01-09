"use client";

import { Button } from "flowbite-react";
import React from "react";
import { FieldValues, useForm } from "react-hook-form";
import TextField from "../components/text-field/text-field";
import DateInput from "../components/date-input/date-input";
import { createAuction } from "../services/auction.service";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

const AuctionForm = () => {
  const router = useRouter();
  const {control, handleSubmit, formState: { isSubmitting, isValid }} = useForm({
    mode: 'onTouched'
  });
  const onSubmit = async (data: FieldValues) => {
    try {
      const res = await createAuction(data);
      if(res.error){
        throw res.error;
      }
      router.push(`/auctions/details/${res.id}`)
    } catch (error) {
      toast.error(error.status + ' ' + error.message);
    }
  };

  return (
    <form className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
        <TextField label="Make" name="make" control={control} rules={{required: 'Make is required'}}/>
        <TextField label="Model" name="model" control={control} rules={{required: 'Model is required'}}/>
        <TextField label="Color" name="color" control={control} rules={{required: 'Color is required'}}/>
    
        <div className="grid grid-cols-2 gap-3">
            <TextField label="Year" name="year" control={control} 
                rules={{required: 'Color is required'}} type="number"/>
            <TextField label="Milage" name="milage" control={control} rules={{required: 'Milage is required'}}/>
        </div>
        <TextField label="Image URL" name="imageUrl" control={control} rules={{required: 'Image url is required'}}/>

        <div className="grid grid-cols-2 gap-3">
            <TextField label="Reserve price (enter 0 if no reserve)" name="reservePrice" control={control} 
                rules={{required: 'Reserve price is required'}} type="number"/>
            <DateInput label="Auction end date/time" name="auctionEnd" control={control} 
                rules={{required: 'Auction end date is required'}}  dateFormat="dd MMMM yyyy h:m a" showTimeSelect/>
        </div>

      <div className="flex justify-between">
        <Button outline color="gray">
          Cancel
        </Button>
        <Button disabled={!isValid} isProcessing={isSubmitting} type="submit" outline color="success">
          Submit
        </Button>
      </div>
    </form>
  );
};

export default AuctionForm;
