"use client";
import { deleteAuction } from "@/app/services/auction.service";
import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import React, { useState } from "react";
import toast from "react-hot-toast";

interface Props {
  id: string;
}

const DeleteButton = ({ id }: Props) => {
  const [loading, setLoading] = useState<boolean>(false);
  const router = useRouter();

  const doDelete = async () => {
    setLoading(true);
    try {
      const res = await deleteAuction(id);
      if (res.error) throw res.error;
      toast.success("Auction deleted successfully");
      router.push("/");
    } catch (error) {
      toast.error(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Button color="failure" isProcessing={loading} onClick={doDelete}>
      Delet acution
    </Button>
  );
};

export default DeleteButton;
