import { Button } from "flowbite-react";
import Link from "next/link";
import React from "react";

interface Props {
  id: string;
}

const EditButton = ({id}: Props) => {
  return (
    <Button outline color="info">
      <Link href={`/auctions/update/${id}`}>Update Auction</Link>
    </Button>
  );
};

export default EditButton;
